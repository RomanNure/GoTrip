package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.UserRegistrationFormDto;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.util.session.AppSession;
import org.nure.gotrip.util.session.SessionContainer;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletResponse;

@RestController
public class AuthorizationController {

	private final Logger logger = LoggerFactory.getLogger(AuthorizationController.class);

	private RegisteredUserService registeredUserService;

	private SessionContainer sessionContainer;

	@Autowired
	public AuthorizationController(RegisteredUserService registeredUserService, SessionContainer sessionContainer) {
		this.registeredUserService = registeredUserService;
        this.sessionContainer = sessionContainer;
    }

	@PostMapping(value = "/authorize", produces = "application/json")
	public RegisteredUser authorize(HttpServletResponse response, @RequestBody UserRegistrationFormDto userRegistrationFormDto) {
		RegisteredUser user = getRegisteredUser(userRegistrationFormDto);
		checkUserPassword(user, userRegistrationFormDto);
		String sessionId = sessionContainer.createSession();
		AppSession session = sessionContainer.getSession(sessionId);
		session.setAttribute("user", user);
		Cookie cookie = new Cookie("SESSIONID", sessionId);
		cookie.setMaxAge(60*60*3);
		response.addCookie(cookie);
		return user;
	}

	private RegisteredUser getRegisteredUser(UserRegistrationFormDto userRegistrationFormDto) {
		try {
			return registeredUserService.findByLogin(userRegistrationFormDto.getLogin());
		} catch (NotFoundUserException e) {
			logger.info(e.getMessage());
			throw new NotFoundException(e.getMessage());
		}
	}

	private void checkUserPassword(RegisteredUser user, UserRegistrationFormDto userRegistrationFormDto) {
		if (!registeredUserService.checkPassword(user, userRegistrationFormDto.getPassword())) {
			String logMessage = String.format("Invalid password entered for user %s", userRegistrationFormDto.getLogin());
			logger.info(logMessage);
			throw new BadRequestException("Invalid password");
		}
	}

}