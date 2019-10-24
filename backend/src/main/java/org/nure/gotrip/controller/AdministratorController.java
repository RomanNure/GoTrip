package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.AdministratorAddDto;
import org.nure.gotrip.dto.AdministratorDto;
import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueAdministratorException;
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
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import java.util.ArrayList;
import java.util.List;

@Controller
@RequestMapping("/administrator")
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

    @GetMapping(value = "/get", produces = "application/json")
    public ResponseEntity getAdministrators(@RequestParam long companyId){
        Company company;
        try {
            company = companyService.findById(companyId);
        } catch (NotFoundCompanyException e) {
            throw new NotFoundException(e.getMessage());
        }
        List<AdministratorDto> list = new ArrayList<>();
        company.getAdministrators().forEach(admin -> list.add(new AdministratorDto(admin.getId(), registeredUserService.findByAdministrator(admin.getId()))));
        return new ResponseEntity<>(list, HttpStatus.OK);
    }

    @PostMapping(value = "/add", produces = "application/json")
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
        try {
            administrator = administratorService.addAdministrator(administrator);
            return new ResponseEntity<>(administrator, HttpStatus.OK);
        }catch (NotUniqueAdministratorException e){
            LOGGER.info("Administrator exists", e);
            throw new ConflictException(e.getMessage());
        }
    }

}
