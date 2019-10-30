package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.AdministratorAddDto;
import org.nure.gotrip.dto.AdministratorDto;
import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueAdministratorException;
import org.nure.gotrip.exception.ValidationException;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.AdministratorService;
import org.nure.gotrip.service.CompanyService;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.util.validation.AdministratorAddValidator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

@Controller
@RequestMapping("/administrator")
public class AdministratorController {

	private static final Logger LOGGER = LoggerFactory.getLogger(AdministratorController.class);

	private AdministratorService administratorService;
	private RegisteredUserService registeredUserService;
	private CompanyService companyService;
	private AdministratorAddValidator administratorAddValidator;

	@Autowired
	public AdministratorController(AdministratorService administratorService, RegisteredUserService registeredUserService,
	                               CompanyService companyService, AdministratorAddValidator administratorAddValidator) {
		this.administratorService = administratorService;
		this.registeredUserService = registeredUserService;
		this.companyService = companyService;
		this.administratorAddValidator = administratorAddValidator;
	}

	@GetMapping(value = "/get", produces = "application/json")
	public ResponseEntity getAdministrators(@RequestParam long companyId) {
		Company company;
		try {
			company = companyService.findById(companyId);
		} catch (NotFoundCompanyException e) {
			throw new NotFoundException(e.getMessage());
		}
		List<AdministratorDto> list = new ArrayList<>();
		company.getAdministrators().forEach(admin -> list.add(new AdministratorDto(admin.getId(), registeredUserService.findByAdministrator(admin.getId()))));
		return new ResponseEntity<>(list, HttpStatus.OK);
	}

	@PostMapping(value = "/add", produces = "application/json")
	public ResponseEntity addAdministrator(@RequestBody AdministratorAddDto administratorDto) {
		try {
			return addAdministratorHandler(administratorDto);
		} catch (NotFoundUserException e) {
			LOGGER.info("User not found", e);
			throw new NotFoundException(e.getMessage());
		} catch (NotFoundCompanyException e) {
			LOGGER.info("Company not found", e);
			throw new NotFoundException(e.getMessage());
		} catch (NotUniqueAdministratorException e) {
			LOGGER.info("Administrator exists", e);
			throw new ConflictException(e.getMessage());
		} catch (ValidationException e) {
			LOGGER.info("Invalid request data");
			throw new BadRequestException(e.getMessage());
		}
	}

	private ResponseEntity addAdministratorHandler(AdministratorAddDto administratorDto) throws NotFoundUserException, NotFoundCompanyException,
			NotUniqueAdministratorException, ValidationException {
		RegisteredUser user = getRegisteredUserFromAdministratorAddDto(administratorDto);
		Company company = companyService.findById(administratorDto.getCompanyId());
		Administrator administrator = new Administrator(user, company);
		administrator = administratorService.addAdministrator(administrator);
		return new ResponseEntity<>(administrator, HttpStatus.OK);
	}

	private RegisteredUser getRegisteredUserFromAdministratorAddDto(AdministratorAddDto administratorAddDto) throws NotFoundUserException, ValidationException {
		if (Objects.nonNull(administratorAddDto.getEmail())) {
			administratorAddValidator.validateEmail(administratorAddDto.getEmail());
			return registeredUserService.findByEmail(administratorAddDto.getEmail());
		} else {
			administratorAddValidator.validateLogin(administratorAddDto.getLogin());
			return registeredUserService.findByLogin(administratorAddDto.getLogin());
		}
	}

	@GetMapping(value = "/get/user", produces = "application/json")
	public ResponseEntity<RegisteredUser> getAdministratorWithUserInfo(@RequestParam long id) {
		RegisteredUser registeredUser = registeredUserService.findByAdministrator(id);
		if (registeredUser == null) {
			throw new NotFoundException("User with such admin id was not found");
		}
		return new ResponseEntity<>(registeredUser, HttpStatus.OK);
	}

}
