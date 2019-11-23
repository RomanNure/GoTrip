package org.nure.gotrip.controller;

import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.Getter;
import lombok.Setter;
import org.nure.gotrip.controller.response.ForbiddenException;
import org.nure.gotrip.dto.AbilityParticipatingDto;
import org.nure.gotrip.dto.ParticipatingDto;
import org.nure.gotrip.dto.PreparingDto;
import org.nure.gotrip.model.Participating;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.ParticipatingService;
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

import static java.lang.String.format;

@Controller
@RequestMapping("/participating")
public class ParticipatingController {

    private static final Logger LOGGER = LoggerFactory.getLogger(ParticipatingController.class);
    private static final String[] keys = {
        "sandbox_i74310151520", "sandbox_kgwzzF9TsmUOIJmQQeQyM4G4yrxfGJxVq64k8hLn"
    };

    private ParticipatingService participatingService;
    private SessionContainer sessionContainer;
    private Encoder encoder;

    @Autowired
    private ParticipatingController(ParticipatingService participatingService, SessionContainer sessionContainer, Encoder encoder){
        this.participatingService = participatingService;
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
        Participating participating = participatingService.participate(user.getId(), dto.getTourId());
        return new ResponseEntity<>(participating, HttpStatus.OK);
    }

    @PostMapping(value="/prepare")
    public ResponseEntity prepare(@CookieValue(name = "SESSIONID") String sessionId, @RequestBody PreparingDto dto){
        AppSession session = checkSession(sessionId);
        RegisteredUser user = checkUser(session);
        dto.setUserId(user.getId());
        boolean result = participatingService.prepare(dto);
        return new ResponseEntity<>("{\"added\":\"" + result + "\"}", HttpStatus.OK);
    }

    @PostMapping(value="/add/liqpay")
    public void confirm(@RequestParam String data, @RequestParam String signature) throws IOException {
        String received = encoder.decodeBase64(signature);
        String serverSignature = encoder.encodeSHA1(keys[1] + data + keys[1]);
        if(received.equals(serverSignature)){
            String jsonData = encoder.decodeBase64(data);
            LiqPayRequest request = new ObjectMapper().readValue(jsonData, LiqPayRequest.class);
            if(request.isSuccess()) {
                PreparingDto dto = participatingService.confirm(request.getOrder_id());
                participatingService.participate(dto.getUserId(), dto.getTourId());
            }else{
                LOGGER.info(format("Order with id %s was not successful", request.getOrder_id()));
            }
        }
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
    private static class LiqPayRequest {

        private String status;
        private String order_id;

        boolean isSuccess() {
            return status.equals("success");
        }
    }
}
