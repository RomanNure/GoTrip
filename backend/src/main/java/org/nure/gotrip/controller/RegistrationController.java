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
import org.nure.gotrip.util.session.AppSession;
import org.nure.gotrip.util.session.SessionContainer;
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
import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.IOException;

@RestController
public class RegistrationController {

	private final Logger logger = LoggerFactory.getLogger(RegistrationController.class);

	private RegisteredUserService registeredUserService;
	private MailService mailService;
	private ModelMapper modelMapper;
	private RegistrationUserFormValidator registrationUserFormValidator;

    private SessionContainer sessionContainer;

	@Autowired
	public RegistrationController(RegisteredUserService registeredUserService, MailService mailService,
	                              ModelMapper modelMapper, RegistrationUserFormValidator registrationUserFormValidator) {
		this.registeredUserService = registeredUserService;
		this.mailService = mailService;
		this.modelMapper = modelMapper;
		this.registrationUserFormValidator = registrationUserFormValidator;
	}

	@PostMapping(value = "/register", produces = "application/json")
	public ResponseEntity signUp(HttpServletResponse response, @RequestBody UserRegistrationFormDto userRegistrationFormDto) {
		try {
			registrationUserFormValidator.registrationUserFormValid(userRegistrationFormDto);
			RegisteredUser user = registeredUserService.add(
					modelMapper.map(userRegistrationFormDto, RegisteredUser.class)
			);
			sendMail(user);
            String sessionId = sessionContainer.createSession();
            AppSession session = sessionContainer.getSession(sessionId);
            session.setAttribute("user", user);
            Cookie cookie = new Cookie("SESSIONID", sessionId);
            cookie.setMaxAge(60*60*3);
            response.addCookie(cookie);
			return new ResponseEntity<>(user, HttpStatus.OK);
		} catch (ValidationException e) {
			logger.info(e.getMessage());
			throw new BadRequestException("Invalid field value");
		}catch (NotUniqueUserException e){
			logger.info(e.getMessage());
			throw new ConflictException("User with such login or email already exists");
		}
	}

	private void sendMail(RegisteredUser user) {
		new Thread(() -> {
			try {
				mailService.sendThroughRemote(user.getEmail(),
                        mailService.getMailTemplate("target/classes/mail.html"),
                        "Email Confirmation",
                        user.getLogin(),
                        mailService.getEmailProperty("mailAddress"),
                        String.valueOf(user.getId())
                );
			} catch (MessagingException | IOException e) {
				logger.error("Exception while sending email", e);
			}
		}).start();
	}

}