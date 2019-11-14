package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.ForbiddenException;
import org.nure.gotrip.dto.ParticipatingDto;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.ParticipatingService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

import javax.servlet.http.HttpSession;

@Controller
@RequestMapping("/participating")
public class ParticipatingController {

    private ParticipatingService participatingService;

    @Autowired
    private ParticipatingController(ParticipatingService participatingService){
        this.participatingService = participatingService;
    }

    @GetMapping(value = "/able", produces = "application/json")
    public ResponseEntity isAbleToParticipate(long userId, long tourId){
        return new ResponseEntity<>(participatingService.isAbleToParticipate(userId, tourId), HttpStatus.OK);
    }

    @PostMapping(value="/add")
    public ResponseEntity participate(HttpSession session, @RequestBody ParticipatingDto dto){
        RegisteredUser user = (RegisteredUser)session.getAttribute("user");
        if(user == null){
            throw new ForbiddenException("Not authorized user");
        }
        participatingService.participate(user.getId(), dto.getTourId());
        return new ResponseEntity(HttpStatus.OK);
    }
}
