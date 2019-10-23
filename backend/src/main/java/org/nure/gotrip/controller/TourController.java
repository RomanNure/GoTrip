package org.nure.gotrip.controller;

import org.nure.gotrip.model.Tour;
import org.nure.gotrip.service.TourService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

import java.util.List;

@Controller
public class TourController {

    private TourService tourService;

    @Autowired
    public TourController(TourService tourService) {
        this.tourService = tourService;
    }

    @GetMapping(value = "/tours/get", produces = "application/json")
    public ResponseEntity<List<Tour>> getAll(){
        return new ResponseEntity<>(tourService.findAll(), HttpStatus.OK);
    }
}
