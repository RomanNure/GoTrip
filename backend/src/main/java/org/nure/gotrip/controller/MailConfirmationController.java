package org.nure.gotrip.controller;

import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.RegisteredUserService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class MailConfirmationController {

    private static final Logger logger = LoggerFactory.getLogger(MailConfirmationController.class);

    private RegisteredUserService userService;

    @Autowired
    public MailConfirmationController(RegisteredUserService userService) {
        this.userService = userService;
    }

    @PostMapping("/user/email/confirm")
    public void confirmEmail(@RequestParam long userId){
        RegisteredUser user;
        try {
            user = userService.findById(userId);
        } catch (NotFoundUserException e) {
            logger.info("Cannot find user", e);
            return;
        }
        user.setEmailConfirmed(true);
        userService.update(user);
    }
}
