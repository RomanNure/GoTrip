package org.nure.gotrip.controller;

import org.modelmapper.ModelMapper;
import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.ForbiddenException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.DoubleDto;
import org.nure.gotrip.dto.FilterUnit;
import org.nure.gotrip.dto.TourDto;
import org.nure.gotrip.exception.NotFoundAdministratorException;
import org.nure.gotrip.exception.NotFoundTourException;
import org.nure.gotrip.exception.NotUniqueTourException;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.model.TourPhoto;
import org.nure.gotrip.service.AdministratorService;
import org.nure.gotrip.service.TourService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import java.util.Arrays;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.stream.Collectors;

@Controller
@RequestMapping("/tours")
public class TourController {

    private TourService tourService;
    private AdministratorService administratorService;
    private ModelMapper modelMapper;

    @Autowired
    public TourController(TourService tourService, AdministratorService administratorService, ModelMapper modelMapper) {
        this.tourService = tourService;
        this.administratorService = administratorService;
        this.modelMapper = modelMapper;
    }

    @GetMapping(value = "/get", produces = "application/json")
    public ResponseEntity<List<Tour>> getAll() {
        return new ResponseEntity<>(tourService.findAll(), HttpStatus.OK);
    }

    @GetMapping(value = "/get/id", produces = "application/json")
    public ResponseEntity<Tour> getById(@RequestParam long id) {
        try {
            Tour tour = tourService.findById(id);
            return new ResponseEntity<>(tour, HttpStatus.OK);
        } catch (NotFoundTourException e) {
            throw new NotFoundException(e.getMessage());
        }
    }

    @PostMapping(value = "/add", produces = "application/json")
    public ResponseEntity addTour(@RequestBody TourDto tourDto) {
        try {
            Tour tour = getTour(tourDto);
            Tour newTour = tourService.add(tour);
            return new ResponseEntity<>(newTour, HttpStatus.OK);
        } catch (NotUniqueTourException e) {
            throw new ConflictException(e.getMessage());
        } catch (NotFoundAdministratorException e) {
            throw new NotFoundException(e.getMessage());
        }
    }

    @PostMapping(value = "/edit", produces = "application/json")
    public ResponseEntity editTour(@RequestBody Tour tour) {
        try {
            tourService.update(tour);
        } catch (NotFoundTourException e) {
            throw new NotFoundException("Can't find such tour");
        }
        return new ResponseEntity(HttpStatus.OK);
    }

    @PostMapping(value = "/get/advanced", produces = "application/json")
    public ResponseEntity getByFilters(@RequestBody FilterUnit filterUnit) {
        return new ResponseEntity<>(tourService.getByCriteria(filterUnit), HttpStatus.OK);
    }

    @GetMapping(value = "/get/users")
    public ResponseEntity getUsers(long id){
        return new ResponseEntity<>(tourService.getByTour(id), HttpStatus.OK);
    }

    @GetMapping(value = "/get/average")
    public ResponseEntity getTourAverage(long id){
        try {
            Tour tour = tourService.findById(id);
            AtomicInteger all = new AtomicInteger(0);
            AtomicInteger sum = new AtomicInteger(0);
            tour.getParticipatingList()
                    .forEach(participating -> {
                        if(participating.isFinished()){
                            sum.addAndGet(participating.getTourRate());
                            all.incrementAndGet();
                        }
                    });
            if(all.intValue() == 0){
                return new ResponseEntity<>(new DoubleDto(0.0), HttpStatus.OK);
            }
            return new ResponseEntity<>(new DoubleDto(sum.doubleValue()/all.doubleValue()), HttpStatus.OK);
        } catch (NotFoundTourException e) {
            throw new NotFoundException(e.getMessage());
        }
    }

    private Tour getTour(TourDto tourDto) throws NotFoundAdministratorException {
        Administrator administrator = administratorService.getById(tourDto.getIdAdministrator());

        boolean isCustom = administrator.getTours().stream()
                .anyMatch(Tour::isCustom);
        if(isCustom){
            throw new ForbiddenException("Can't create tour by this administrator");
        }

        Tour tour = modelMapper.map(tourDto, Tour.class);
        tour.setPhotos(Arrays.stream(tourDto.getPhotosUrl())
                .map(stringUrl -> new TourPhoto(stringUrl, tour))
                .collect(Collectors.toList()));
        tour.setAdministrator(administrator);
        return tour;
    }
}
