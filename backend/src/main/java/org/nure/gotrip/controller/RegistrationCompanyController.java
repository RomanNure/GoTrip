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
import org.nure.gotrip.service.impl.MailService;
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

import javax.mail.MessagingException;
import java.io.IOException;

@RestController
@RequestMapping("/company")
public class RegistrationCompanyController {

	private static final Logger logger = LoggerFactory.getLogger(RegistrationCompanyController.class);

	private CompanyService companyService;
	private CompanyRegistrationValidator companyRegistrationValidator;
	private ModelMapper modelMapper;
	private MailService mailService;

	@Autowired
	public RegistrationCompanyController(CompanyService companyService, CompanyRegistrationValidator companyRegistrationValidator, ModelMapper modelMapper, MailService mailService) {
		this.companyService = companyService;
		this.companyRegistrationValidator = companyRegistrationValidator;
		this.modelMapper = modelMapper;
		this.mailService = mailService;
	}

	@PostMapping(value = "/registration", produces = "application/json")
	public ResponseEntity registrationCompany(@RequestBody CompanyDto companyDto) {
		try {
			companyRegistrationValidator.registrationCompanyValid(companyDto);
			Company company = companyService.add(modelMapper.map(companyDto, Company.class));
			sendEmail(company);
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

	private void sendEmail(Company company) {
		new Thread(() -> sendEmailHandler(company)).start();
	}

	private void sendEmailHandler(Company company) {
		try {
			mailService.sendThroughRemote(company.getEmail(),
					mailService.getMailTemplate("target/classes/company.html"),
					"A new company",
					mailService.getEmailProperty("companyAddress"),
					company.getName()
			);
		} catch (MessagingException | IOException e) {
			logger.error("Exception while sending email", e);
		}
	}

}