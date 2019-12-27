package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.AddGuideDto;
import org.nure.gotrip.dto.DoubleDto;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueGuideException;
import org.nure.gotrip.exception.ValidationException;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.GuideService;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.util.validation.GuideAddValidator;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.concurrent.atomic.AtomicInteger;

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
		} catch (NotUniqueGuideException e) {
			throw new ConflictException("User with such id already exists");
		}
	}

	@GetMapping(value="/get/all", produces = "application/json")
    public ResponseEntity getGuides(){
	    Iterable<Guide> result = guideService.getAll();
	    return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping(value="/get", produces = "application/json")
    public ResponseEntity getGuide(long id) {
        Guide guide = guideService.getById(id).orElseThrow(() -> new NotFoundException("Guide not found"));
        return new ResponseEntity<>(guide, HttpStatus.OK);
    }

    @GetMapping(value="/get/average", produces = "application/json")
    public ResponseEntity getGuideAverage(long id) {
        Guide guide = guideService.getById(id).orElseThrow(() -> new NotFoundException("Guide not found"));
        AtomicInteger all = new AtomicInteger(0);
        AtomicInteger sum = new AtomicInteger(0);

        guide.getTours().forEach(tour -> tour.getParticipatingList().forEach(participating -> {
            if (participating.isFinished()){
                all.incrementAndGet();
                sum.addAndGet(participating.getGuideRate());
            }
        }));
        if(all.intValue() == 0){
            return new ResponseEntity<>(new DoubleDto(0.0), HttpStatus.OK);
        }
        return new ResponseEntity<>(new DoubleDto(sum.doubleValue()/all.doubleValue()), HttpStatus.OK);
    }

	private ResponseEntity addGuideHandler(AddGuideDto addGuideDto) throws NotFoundUserException, NotUniqueGuideException {
		Guide guide = new Guide();
		RegisteredUser registeredUser = registeredUserService.findById(addGuideDto.getIdRegisteredUser());
		guide.setCardNumber(addGuideDto.getCardNumber());
		guide.setRegisteredUser(registeredUser);
		guide.setWantedToursKeyWords(addGuideDto.getWantedToursKeyWords());
		Guide newGuide = guideService.add(guide);
		return new ResponseEntity<>(newGuide, HttpStatus.OK);
	}

}