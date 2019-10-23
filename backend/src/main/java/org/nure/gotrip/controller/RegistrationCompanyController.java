package org.nure.gotrip.controller;

import org.modelmapper.ModelMapper;
import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.CompanyDto;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueCompanyException;
import org.nure.gotrip.exception.ValidationException;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.service.CompanyService;
import org.nure.gotrip.util.validation.CompanyRegistrationValidator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/company")
public class RegistrationCompanyController {

	private static final Logger logger = LoggerFactory.getLogger(RegistrationCompanyController.class);

	private CompanyService companyService;
	private CompanyRegistrationValidator companyRegistrationValidator;
	private ModelMapper modelMapper;

	@Autowired
	public RegistrationCompanyController(CompanyService companyService, CompanyRegistrationValidator companyRegistrationValidator, ModelMapper modelMapper) {
		this.companyService = companyService;
		this.companyRegistrationValidator = companyRegistrationValidator;
		this.modelMapper = modelMapper;
	}

	@PostMapping(value = "/registration", produces = "application/json")
	public ResponseEntity registrationCompany(@RequestBody CompanyDto companyDto) {
		try {
			companyRegistrationValidator.registrationCompanyValid(companyDto);
			Company company = companyService.add(modelMapper.map(companyDto, Company.class));
			return new ResponseEntity<>(company, HttpStatus.OK);
		} catch (ValidationException e) {
			logger.warn(e.getMessage(), e);
			throw new BadRequestException("Invalid field value");
		} catch (NotUniqueCompanyException e) {
			throw new ConflictException(e.getMessage());
		} catch (NotFoundUserException e) {
			throw new NotFoundException(e.getMessage());
		}
	}

}
