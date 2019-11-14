package org.nure.gotrip.controller;

import org.nure.gotrip.dto.ParticipatingDto;
import org.nure.gotrip.service.ParticipatingService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

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
    public ResponseEntity participate(@RequestBody ParticipatingDto dto){
        participatingService.participate(dto.getUserId(), dto.getTourId());
        return new ResponseEntity(HttpStatus.OK);
    }
}
