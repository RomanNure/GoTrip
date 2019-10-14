package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.RegisteredUserService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class UserInfoController {

    private static final Logger logger = LoggerFactory.getLogger(UserInfoController.class);

    private RegisteredUserService registeredUserService;

    @Autowired
    public UserInfoController(RegisteredUserService registeredUserService) {
        this.registeredUserService = registeredUserService;
    }

    @GetMapping("/user/get")
    public RegisteredUser getUserInfo(@RequestParam long userId){
        try {
            return registeredUserService.findById(userId);
        } catch (NotFoundUserException e) {
            logger.info(e.getMessage());
            throw new NotFoundException(e.getMessage());
        }
    }
}
