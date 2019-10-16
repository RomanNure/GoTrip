package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.UserRegistrationFormDto;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.RegisteredUserService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class AuthorizationController {

    private final Logger logger = LoggerFactory.getLogger(AuthorizationController.class);

    private RegisteredUserService registeredUserService;

    @Autowired
    public AuthorizationController(RegisteredUserService registeredUserService) {
        this.registeredUserService = registeredUserService;
    }

    @PostMapping(value = "/authorize", produces = "application/json")
    public RegisteredUser authorize(@RequestBody UserRegistrationFormDto userRegistrationFormDto) {
        RegisteredUser user;

        try {
            user = registeredUserService.findByLogin(userRegistrationFormDto.getLogin());
        } catch (NotFoundUserException e) {
            logger.info(e.getMessage());
            throw new NotFoundException(e.getMessage());
        }

        if (!registeredUserService.checkPassword(user, userRegistrationFormDto.getPassword())) {
            String logMessage = String.format("Invalid password entered for user %s", userRegistrationFormDto.getLogin());
            logger.info(logMessage);
            throw new BadRequestException("Invalid password");
        }

        return user;
    }
}
