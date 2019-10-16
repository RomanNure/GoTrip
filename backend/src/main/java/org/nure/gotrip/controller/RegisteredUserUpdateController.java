package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.ValidationException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.util.validation.RegistrationUserFormValidator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class RegisteredUserUpdateController {

    private final Logger logger = LoggerFactory.getLogger(RegisteredUserUpdateController.class);

    private RegisteredUserService registeredUserService;
    private RegistrationUserFormValidator registrationUserFormValidator;

    @Autowired
    public RegisteredUserUpdateController(RegisteredUserService registeredUserService, RegistrationUserFormValidator registrationUserFormVal) {
        this.registeredUserService = registeredUserService;
        this.registrationUserFormValidator = registrationUserFormVal;
    }

    @PostMapping(value = "/update/user", produces = "application/json")
    public RegisteredUser updateUserData(@RequestBody RegisteredUser updatedUser){
        try {
            registrationUserFormValidator.validateUser(updatedUser);
        } catch (ValidationException e) {
            logger.info("Invalid request data");
            throw new BadRequestException(e.getMessage());
        }

        long userId = updatedUser.getId();
        RegisteredUser oldUser;
        try {
            oldUser = registeredUserService.findById(userId);
        } catch (NotFoundUserException e) {
            logger.info("User with invalid id tried to update");
            throw new BadRequestException("Invalid id");
        }

        oldUser.setEmail(updatedUser.getEmail());
        oldUser.setPhone(updatedUser.getPhone());
        oldUser.setFullName(updatedUser.getFullName());
        oldUser.setAvatarUrl(updatedUser.getAvatarUrl());

        registeredUserService.update(oldUser);
        return oldUser;
    }
}
