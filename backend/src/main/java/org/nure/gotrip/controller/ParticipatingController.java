package org.nure.gotrip.controller;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.Getter;
import lombok.Setter;
import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.ForbiddenException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.*;
import org.nure.gotrip.exception.NotFoundParticipatingException;
import org.nure.gotrip.exception.NotFoundTourException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.Participating;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.service.ParticipatingService;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.service.TourService;
import org.nure.gotrip.util.Encoder;
import org.nure.gotrip.util.session.AppSession;
import org.nure.gotrip.util.session.SessionContainer;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.UUID;

import static java.lang.String.format;

@Controller
@RequestMapping("/participating")
public class ParticipatingController {

    private static final Logger LOGGER = LoggerFactory.getLogger(ParticipatingController.class);
    private static final String[] keys = {
            "sandbox_i74310151520", "sandbox_kgwzzF9TsmUOIJmQQeQyM4G4yrxfGJxVq64k8hLn"
    };
    private static final String HASH_KEY = "2019GoNTrip2019";

    private ParticipatingService participatingService;
    private TourService tourService;
    private RegisteredUserService registeredUserService;
    private SessionContainer sessionContainer;
    private Encoder encoder;

    @Autowired
    private ParticipatingController(ParticipatingService participatingService, TourService tourService, RegisteredUserService registeredUserService, SessionContainer sessionContainer, Encoder encoder) {
        this.participatingService = participatingService;
        this.tourService = tourService;
        this.registeredUserService = registeredUserService;
        this.sessionContainer = sessionContainer;
        this.encoder = encoder;
    }

    @GetMapping(value = "/able", produces = "application/json")
    public ResponseEntity isAbleToParticipate(long userId, long tourId) {
        return new ResponseEntity<>(new BooleanDto(
                participatingService.isAbleToParticipate(userId, tourId)
        ), HttpStatus.OK);
    }

    @PostMapping(value = "/add")
    public ResponseEntity participate(@CookieValue(name = "SESSIONID") String sessionId, @RequestBody ParticipatingDto dto) {
        AppSession session = checkSession(sessionId);
        RegisteredUser user = checkUser(session);
        String id = UUID.randomUUID().toString();
        PreparingDto preparingDto = new PreparingDto(id, dto.getTourId(), user.getId());
        Participating participating = participatingService.participate(preparingDto);
        return new ResponseEntity<>(participating, HttpStatus.OK);
    }

    @PostMapping(value = "/prepare")
    public ResponseEntity prepare(@CookieValue(name = "SESSIONID") String sessionId, @RequestBody PreparingDto dto) {
        AppSession session = checkSession(sessionId);
        RegisteredUser user = checkUser(session);
        if (participatingService.isAbleToParticipate(user.getId(), dto.getTourId())) {
            dto.setUserId(user.getId());
            boolean result = participatingService.prepare(dto);
            return new ResponseEntity<>(new BooleanDto(result), HttpStatus.OK);
        }
        throw new ConflictException("You cannot participate at that time");
    }

    @PostMapping(value = "/add/liqpay")
    public ResponseEntity confirm(@RequestParam String data, @RequestParam String signature) throws IOException {
        String received = encoder.decodeBase64Hex(signature);
        String serverSignature = encoder.encodeSHA1(keys[1] + data + keys[1]);
        if (received.equals(serverSignature)) {
            String jsonData = encoder.decodeBase64(data);
            LiqPayRequest request = new ObjectMapper().readValue(jsonData, LiqPayRequest.class);
            if (request.isSuccess()) {
                PreparingDto dto = participatingService.confirm(request.getOrder_id());
                if (dto != null) {
                    participatingService.participate(dto);
                }
            } else {
                LOGGER.info(format("Order with id %s was not successful", request.getOrder_id()));
            }
        }
        return new ResponseEntity(HttpStatus.OK);
    }

    @GetMapping(value = "/get")
    public ResponseEntity getParticipating(@CookieValue(name = "SESSIONID") String sessionId, @RequestParam long tourId) {
        AppSession session = checkSession(sessionId);
        Tour tour;
        try {
            tour = tourService.findById(tourId);
        } catch (NotFoundTourException e) {
            throw new NotFoundException(e.getMessage());
        }
        RegisteredUser user = checkUser(session);
        try {
            Participating participating = participatingService.getByTourAndUser(tour, user);
            return new ResponseEntity<>(participating, HttpStatus.OK);
        } catch (NotFoundParticipatingException e) {
            throw new NotFoundException(e.getMessage());
        }
    }

    @PostMapping(value = "/hash")
    public ResponseEntity checkHash(@CookieValue(name = "SESSIONID") String sessionId, @RequestBody MemberCheckingDto dto) {
        AppSession session = checkSession(sessionId);
        Tour tour;
        try {
            tour = tourService.findById(dto.getTourId());
        } catch (NotFoundTourException e) {
            throw new NotFoundException(e.getMessage());
        }
        checkUser(session);
        RegisteredUser user;
        try {
            user = registeredUserService.findById(dto.getUserId());
        } catch (NotFoundUserException e) {
            throw new NotFoundException(e.getMessage());
        }
        try {
            Participating participating = participatingService.getByTourAndUser(tour, user);
            String hash = getHashCode(participating, dto.getGuideId());
            if (hash.equals(dto.getHash())) {
                participatingService.markParticipated(participating);
                return new ResponseEntity<>(new BooleanDto(true), HttpStatus.OK);
            } else {
                throw new BadRequestException("Not valid hash provided");
            }
        } catch (NotFoundParticipatingException e) {
            throw new NotFoundException(e.getMessage());
        }
    }

    @GetMapping(value = "/check")
    public ResponseEntity checkStatus(@RequestParam String orderId) {
        String status = participatingService.getStatus(orderId);
        return new ResponseEntity<>(new StatusResponseDto(status), HttpStatus.OK);
    }

    private AppSession checkSession(String sessionId) {
        AppSession session = sessionContainer.getSession(sessionId);
        if (session == null) {
            throw new ForbiddenException("Not authorized user");
        }
        return session;
    }

    private RegisteredUser checkUser(AppSession session) {
        RegisteredUser user = (RegisteredUser) session.getAttribute("user");
        if (user == null) {
            throw new ForbiddenException("Not authorized user");
        }
        return user;
    }

    private String getHashCode(Participating participating, long guideId) {
        String template = "%s_%s_%s_%s_%s";
        String compilation = String.format(template,
                HASH_KEY,
                participating.getUser().getId(),
                participating.getTour().getId(),
                participating.getOrderId(),
                HASH_KEY);
        String temp = encoder.encodeMd5(compilation);
        temp = temp.replace("-", "").toUpperCase();
        temp = encoder.encodeMd5(String.format("%d_%s", guideId, temp));
        return temp.replace("-", "").toUpperCase();
    }

    /*participated field by user id, tour id
finished field by user id, tour id*/

    @GetMapping(value = "/status")
    public ResponseEntity checkStatus(@RequestParam long userId, @RequestParam long tourId) {
        try {
            Tour tour = tourService.findById(tourId);
            RegisteredUser user = registeredUserService.findById(userId);

            Participating participating;
            try {
                participating = participatingService.getByTourAndUser(tour, user);
            } catch (NotFoundParticipatingException e) {
                throw new NotFoundException(e.getMessage());
            }
            ParticipatingStatusDto result = new ParticipatingStatusDto();
            result.setFinished(participating.isFinished());
            result.setParticipated(participating.isParticipated());
            return new ResponseEntity<>(result, HttpStatus.OK);
        }catch (NotFoundTourException | NotFoundUserException e) {
            throw new NotFoundException(e.getMessage());
        }
    }

    @Getter
    @Setter
    @JsonIgnoreProperties(ignoreUnknown = true)
    private static class LiqPayRequest {

        private String status;
        private String order_id;

        boolean isSuccess() {
            return status.equals("success");
        }
    }

}