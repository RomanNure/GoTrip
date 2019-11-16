package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.ForbiddenException;
import org.nure.gotrip.dto.ParticipatingDto;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.ParticipatingService;
import org.nure.gotrip.util.session.AppSession;
import org.nure.gotrip.util.session.SessionContainer;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/participating")
public class ParticipatingController {

    private ParticipatingService participatingService;

    private SessionContainer sessionContainer;

    @Autowired
    private ParticipatingController(ParticipatingService participatingService, SessionContainer sessionContainer){
        this.participatingService = participatingService;
        this.sessionContainer = sessionContainer;
    }

    @GetMapping(value = "/able", produces = "application/json")
    public ResponseEntity isAbleToParticipate(long userId, long tourId){
        return new ResponseEntity<>(participatingService.isAbleToParticipate(userId, tourId), HttpStatus.OK);
    }

    @PostMapping(value="/add")
    public ResponseEntity participate(@CookieValue(name = "SESSIONID") String sessionId, @RequestBody ParticipatingDto dto){
        AppSession session = sessionContainer.getSession(sessionId);
        if(session == null){
            throw new ForbiddenException("Not authorized user");
        }
        RegisteredUser user = (RegisteredUser)session.getAttribute("user");
        if(user == null){
            throw new ForbiddenException("Not authorized user");
        }
        participatingService.participate(user.getId(), dto.getTourId());
        return new ResponseEntity(HttpStatus.OK);
    }
}
