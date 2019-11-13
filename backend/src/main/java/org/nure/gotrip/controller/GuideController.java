package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.AddGuideDto;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.ValidationException;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.GuideService;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.util.validation.GuideAddValidator;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/guide")
public class GuideController {

	private GuideService guideService;
	private RegisteredUserService registeredUserService;
	private GuideAddValidator guideAddValidator;

	@Autowired
	public GuideController(GuideService guideService, RegisteredUserService registeredUserService, GuideAddValidator guideAddValidator) {
		this.guideService = guideService;
		this.registeredUserService = registeredUserService;
		this.guideAddValidator = guideAddValidator;
	}


	@PostMapping(value = "/add", produces = "application/json")
	public ResponseEntity addGuide(@RequestBody AddGuideDto addGuideDto) {
		try {
			guideAddValidator.guideValid(addGuideDto);
			return addGuideHandler(addGuideDto);
		} catch (NotFoundUserException e) {
			throw new NotFoundException(e.getMessage(), e);
		} catch (ValidationException e) {
			throw new BadRequestException(e.getMessage(), e);
		}
	}

	private ResponseEntity addGuideHandler(AddGuideDto addGuideDto) throws NotFoundUserException {
		Guide guide = new Guide();
		RegisteredUser registeredUser = registeredUserService.findById(addGuideDto.getIdRegisteredUser());
		guide.setRegisteredUser(registeredUser);
		guide.setWantedToursKeyWords(addGuideDto.getWantedToursKeyWords());
		Guide newGuide = guideService.add(guide);
		return new ResponseEntity<>(newGuide, HttpStatus.OK);
	}

}