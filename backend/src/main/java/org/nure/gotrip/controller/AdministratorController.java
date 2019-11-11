package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.AdministratorAddDto;
import org.nure.gotrip.dto.AdministratorDto;
import org.nure.gotrip.exception.*;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.service.AdministratorService;
import org.nure.gotrip.service.CompanyService;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.service.TourService;
import org.nure.gotrip.service.impl.MailService;
import org.nure.gotrip.util.validation.AdministratorAddValidator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import javax.mail.MessagingException;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Objects;
import java.util.stream.Collectors;

@Controller
@RequestMapping("/administrator")
public class AdministratorController {

    private static final Logger LOGGER = LoggerFactory.getLogger(AdministratorController.class);

    private MailService mailService;
    private AdministratorService administratorService;
    private RegisteredUserService registeredUserService;
    private CompanyService companyService;
    private AdministratorAddValidator administratorAddValidator;
    private TourService tourService;

    @Autowired
    public AdministratorController(MailService mailService, AdministratorService administratorService, RegisteredUserService registeredUserService,
                                   CompanyService companyService, AdministratorAddValidator administratorAddValidator, TourService tourService) {
        this.mailService = mailService;
        this.administratorService = administratorService;
        this.registeredUserService = registeredUserService;
        this.companyService = companyService;
        this.administratorAddValidator = administratorAddValidator;
        this.tourService = tourService;
    }

    @GetMapping(value = "/get", produces = "application/json")
    public ResponseEntity getAdministrators(@RequestParam long companyId) {
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
    public ResponseEntity addAdministrator(@RequestBody AdministratorAddDto administratorDto) {
        try {
            return addAdministratorHandler(administratorDto);
        } catch (NotFoundUserException e) {
            LOGGER.info("User not found", e);
            throw new NotFoundException(e.getMessage());
        } catch (NotFoundCompanyException e) {
            LOGGER.info("Company not found", e);
            throw new NotFoundException(e.getMessage());
        } catch (NotUniqueAdministratorException e) {
            LOGGER.info("Administrator exists", e);
            throw new ConflictException(e.getMessage());
        } catch (ValidationException e) {
            LOGGER.info("Invalid request data");
            throw new BadRequestException(e.getMessage());
        }
    }

    private ResponseEntity addAdministratorHandler(AdministratorAddDto administratorDto) throws NotFoundUserException, NotFoundCompanyException,
            NotUniqueAdministratorException, ValidationException {
        RegisteredUser user = getRegisteredUserFromAdministratorAddDto(administratorDto);
        Company company = companyService.findById(administratorDto.getCompanyId());
        Administrator administrator = new Administrator(user, company);
        administrator = administratorService.addAdministrator(administrator);
        sendEmail(user, company);
        return new ResponseEntity<>(administrator, HttpStatus.OK);
    }

    private RegisteredUser getRegisteredUserFromAdministratorAddDto(AdministratorAddDto administratorAddDto) throws NotFoundUserException, ValidationException {
        if (Objects.nonNull(administratorAddDto.getEmail())) {
            administratorAddValidator.validateEmail(administratorAddDto.getEmail());
            return registeredUserService.findByEmail(administratorAddDto.getEmail());
        } else {
            administratorAddValidator.validateLogin(administratorAddDto.getLogin());
            return registeredUserService.findByLogin(administratorAddDto.getLogin());
        }
    }

    @GetMapping(value = "/get/user", produces = "application/json")
    public ResponseEntity<RegisteredUser> getAdministratorWithUserInfo(@RequestParam long id) {
        RegisteredUser registeredUser = registeredUserService.findByAdministrator(id);
        if (registeredUser == null) {
            throw new NotFoundException("User with such admin id was not found");
        }
        return new ResponseEntity<>(registeredUser, HttpStatus.OK);
    }

    @PostMapping(value = "/remove")
    public ResponseEntity remove(@RequestParam long id) throws NotFoundAdministratorException {
        List<Tour> tours = tourService.findAll();
        List<Tour> activeTours = tours.stream()
                .filter(tour -> tour.getAdministrator().getId() == id
                        && tour.getFinishDateTime().compareTo(new Date(System.currentTimeMillis())) > 0)
                .collect(Collectors.toList());
        if (!activeTours.isEmpty()) {
            return new ResponseEntity<>(activeTours, HttpStatus.NOT_MODIFIED);
        }
        administratorService.remove(id);
        return new ResponseEntity(HttpStatus.OK);
    }

    private void sendEmail(RegisteredUser user, Company company) {
        new Thread(() -> {
            try {
                mailService.sendThroughRemote(user.getEmail(),
                        mailService.getMailTemplate("target/classes/administrator.html"),
                        "A new administrator job",
                        mailService.getEmailProperty("adminAddress"),
                        company.getName()
                );
            } catch (MessagingException | IOException e) {
                LOGGER.error("Exception while sending email", e);
            }
        }).start();
    }
}
