package org.nure.gotrip.controller;

import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.serviсe.RegisteredUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController(value = "/register")
public class RegistrationController {

    private RegisteredUserService registeredUserService;

    @Autowired
    public RegistrationController(RegisteredUserService registeredUserService) {
        this.registeredUserService = registeredUserService;
    }

    @GetMapping
    public RegisteredUser register(){
        RegisteredUser user = new RegisteredUser();
        user.setLogin("myLogin");
        user.setFullName("Кристофор Колумб");
        return user;
    }
}
