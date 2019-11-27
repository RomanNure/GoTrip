package org.nure.gotrip.controller;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.Getter;
import lombok.Setter;
import org.nure.gotrip.controller.response.ForbiddenException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.AbilityParticipatingDto;
import org.nure.gotrip.dto.ParticipatingDto;
import org.nure.gotrip.dto.PreparingDto;
import org.nure.gotrip.dto.StatusResponseDto;
import org.nure.gotrip.exception.NotFoundParticipatingException;
import org.nure.gotrip.exception.NotFoundTourException;
import org.nure.gotrip.model.Participating;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.service.ParticipatingService;
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
import org.springframework.web.bind.annotation.CookieValue;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

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

    private ParticipatingService participatingService;
    private TourService tourService;
    private SessionContainer sessionContainer;
    private Encoder encoder;

    @Autowired
    private ParticipatingController(ParticipatingService participatingService, TourService tourService, SessionContainer sessionContainer, Encoder encoder){
        this.participatingService = participatingService;
        this.tourService = tourService;
        this.sessionContainer = sessionContainer;
        this.encoder = encoder;
    }

    @GetMapping(value = "/able", produces = "application/json")
    public ResponseEntity isAbleToParticipate(long userId, long tourId){
        return new ResponseEntity<>(new AbilityParticipatingDto(
                participatingService.isAbleToParticipate(userId, tourId)
        ), HttpStatus.OK);
    }

    @PostMapping(value="/add")
    public ResponseEntity participate(@CookieValue(name = "SESSIONID") String sessionId, @RequestBody ParticipatingDto dto){
        AppSession session = checkSession(sessionId);
        RegisteredUser user = checkUser(session);
        String id = UUID.randomUUID().toString();
        PreparingDto preparingDto = new PreparingDto(id, dto.getTourId(), user.getId());
        Participating participating = participatingService.participate(preparingDto);
        return new ResponseEntity<>(participating, HttpStatus.OK);
    }

    @PostMapping(value="/prepare")
    public ResponseEntity prepare(@CookieValue(name = "SESSIONID") String sessionId, @RequestBody PreparingDto dto){
        AppSession session = checkSession(sessionId);
        RegisteredUser user = checkUser(session);
        dto.setUserId(user.getId());
        boolean result = participatingService.prepare(dto);
        return new ResponseEntity<>("{\"able\":\"" + result + "\"}", HttpStatus.OK);
    }

    @PostMapping(value="/add/liqpay")
    public void confirm(@RequestParam String data, @RequestParam String signature) throws IOException {
        String received = encoder.decodeBase64Hex(signature);
        String serverSignature = encoder.encodeSHA1(keys[1] + data + keys[1]);
        if(received.equals(serverSignature)){
            String jsonData = encoder.decodeBase64(data);
            LiqPayRequest request = new ObjectMapper().readValue(jsonData, LiqPayRequest.class);
            if(request.isSuccess()) {
                PreparingDto dto = participatingService.confirm(request.getOrder_id());
                if(dto != null) {
                    participatingService.participate(dto);
                }
            }else{
                LOGGER.info(format("Order with id %s was not successful", request.getOrder_id()));
            }
        }
    }

    @GetMapping(value = "/get")
    public ResponseEntity getParticipating(@CookieValue(name = "SESSIONID") String sessionId, @RequestParam long tourId){
        AppSession session = checkSession(sessionId);
        Tour tour;
        try {
            tour = tourService.findById(tourId);
        }catch (NotFoundTourException e){
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

    @GetMapping(value = "/check")
    public ResponseEntity checkStatus(@RequestParam String orderId){
        String status = participatingService.getStatus(orderId);
        return new ResponseEntity<>(new StatusResponseDto(status), HttpStatus.OK);
    }

    private AppSession checkSession(String sessionId){
        AppSession session = sessionContainer.getSession(sessionId);
        if(session == null){
            throw new ForbiddenException("Not authorized user");
        }
        return session;
    }

    private RegisteredUser checkUser(AppSession session){
        RegisteredUser user = (RegisteredUser)session.getAttribute("user");
        if(user == null){
            throw new ForbiddenException("Not authorized user");
        }
        return user;
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