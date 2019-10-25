package org.nure.gotrip.controller;

import org.modelmapper.ModelMapper;
import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.TourDto;
import org.nure.gotrip.exception.NotFoundAdministratorException;
import org.nure.gotrip.exception.NotUniqueTourException;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.service.AdministratorService;
import org.nure.gotrip.service.TourService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

import java.util.List;

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

	@PostMapping(value = "/add", produces = "application/json")
	public ResponseEntity addTour(@RequestBody TourDto tourDto) {
		try {
			Administrator administrator = administratorService.getById(tourDto.getIdAdministrator());
			Tour tour = modelMapper.map(tourDto, Tour.class);
			tour.setAdministrator(administrator);
			tour = tourService.add(tour);
			return new ResponseEntity<>(tour, HttpStatus.OK);
		} catch (NotUniqueTourException e) {
			throw new ConflictException(e.getMessage());
		} catch (NotFoundAdministratorException e) {
			throw new NotFoundException(e.getMessage());
		}
	}

}
