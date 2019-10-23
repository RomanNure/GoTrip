package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.AdministratorAddDto;
import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.AdministratorService;
import org.nure.gotrip.service.CompanyService;
import org.nure.gotrip.service.RegisteredUserService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

@Controller
public class AdministratorController {

    private static final Logger LOGGER = LoggerFactory.getLogger(AdministratorController.class);

    private AdministratorService administratorService;
    private RegisteredUserService registeredUserService;
    private CompanyService companyService;

    @Autowired
    public AdministratorController(AdministratorService administratorService, RegisteredUserService registeredUserService, CompanyService companyService) {
        this.administratorService = administratorService;
        this.registeredUserService = registeredUserService;
        this.companyService = companyService;
    }

    @PostMapping(value = "/administrator/add", produces = "application/json")
    public ResponseEntity addAdministrator(@RequestBody AdministratorAddDto administratorDto){
        RegisteredUser user;
        Company company;
        try {
            user = registeredUserService.findById(administratorDto.getUserId());
            company = companyService.findById(administratorDto.getCompanyId());
        }catch(NotFoundUserException e){
            LOGGER.info("User not found", e);
            throw new NotFoundException(e.getMessage());
        }catch(NotFoundCompanyException e){
            LOGGER.info("Company not found", e);
            throw new NotFoundException(e.getMessage());
        }
        Administrator administrator = new Administrator(user, company);
        return new ResponseEntity<>(administratorService.addAdministrator(administrator), HttpStatus.OK);
    }
}
