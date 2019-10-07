package org.nure.gotrip.controller;

import org.nure.gotrip.dto.RegisteredUserDto;
import org.nure.gotrip.exception.ValidationException;
import org.nure.gotrip.serviсe.RegisteredUserService;
import org.nure.gotrip.model.RegisteredUser;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.Objects;

@RestController
public class RegistrationController {

    private final Logger logger = LoggerFactory.getLogger(RegistrationController.class);

    private RegisteredUserService registeredUserService;

    private RegisteredUserDto registeredUserDto;

    @Autowired
    public RegistrationController(RegisteredUserService registeredUserService, RegisteredUserDto registeredUserDto) {
        this.registeredUserService = registeredUserService;
        this.registeredUserDto = registeredUserDto;
    }

    @PostMapping(value = "/register")
    public ResponseEntity signUp(
            @RequestParam String login,
            @RequestParam String password,
            @RequestParam String email
    ){
        RegisteredUser user;
        try {
            user = registeredUserDto.get(login, password, email);
        }catch (ValidationException e){
            logger.info(e.getMessage());
            return ResponseEntity.status(HttpStatus.BAD_REQUEST).build();
        }
        user = registeredUserService.add(user);

        ResponseEntity<RegisteredUser> wellResponse = new ResponseEntity<>(user, HttpStatus.OK);
        Objects.requireNonNull(wellResponse.getBody()).setPassword(null);
        return wellResponse;
    }
}
