package org.nure.gotrip.controller;

import org.modelmapper.ModelMapper;
import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.dto.UserRegistrationFormDto;
import org.nure.gotrip.exception.NotUniqueUserException;
import org.nure.gotrip.exception.ValidationException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.service.impl.MailService;
import org.nure.gotrip.util.validation.RegistrationUserFormValidator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import javax.mail.MessagingException;

@RestController
public class RegistrationController {

	private final Logger logger = LoggerFactory.getLogger(RegistrationController.class);

	private RegisteredUserService registeredUserService;
	private MailService mailService;
	private ModelMapper modelMapper;
	private RegistrationUserFormValidator registrationUserFormValidator;

	@Autowired
	public RegistrationController(RegisteredUserService registeredUserService, MailService mailService, ModelMapper modelMapper, RegistrationUserFormValidator registrationUserFormValidator) {
		this.registeredUserService = registeredUserService;
        this.mailService = mailService;
        this.modelMapper = modelMapper;
		this.registrationUserFormValidator = registrationUserFormValidator;
	}

	@PostMapping(value = "/register", produces = "application/json")
	public ResponseEntity signUp(@RequestBody UserRegistrationFormDto userRegistrationFormDto) {
		try {
			registrationUserFormValidator.registrationUserFormValid(userRegistrationFormDto);
			RegisteredUser user = registeredUserService.add(
			        modelMapper.map(userRegistrationFormDto, RegisteredUser.class)
            );
			new Thread(()->{
                try {
                    mailService.sendThroughRemote(user.getEmail(), user.getLogin(), String.valueOf(user.getId()));
                } catch (MessagingException e) {
                    logger.error("Exception while sending email", e);
                }
            }).start();
			return new ResponseEntity<>(user, HttpStatus.OK);
		} catch (ValidationException e) {
			logger.info(e.getMessage());
			throw new BadRequestException("Invalid field value");
		}catch (NotUniqueUserException e){
            logger.info(e.getMessage());
            throw new ConflictException("User with such login already exists");
        }
	}
}
