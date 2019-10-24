package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.CompanyService;
import org.nure.gotrip.service.RegisteredUserService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.math.BigInteger;
import java.util.ArrayList;
import java.util.List;

@RestController
public class UserInfoController {

    private static final Logger logger = LoggerFactory.getLogger(UserInfoController.class);

    private RegisteredUserService registeredUserService;
    private CompanyService companyService;

    @Autowired
    public UserInfoController(RegisteredUserService registeredUserService, CompanyService companyService) {
        this.registeredUserService = registeredUserService;
        this.companyService = companyService;
    }

    @GetMapping("/user/get")
    public RegisteredUser getUserInfo(@RequestParam long id){
        try {
            return registeredUserService.findById(id);
        } catch (NotFoundUserException e) {
            logger.info(e.getMessage());
            throw new NotFoundException(e.getMessage());
        }
    }

    @GetMapping("/user/get/companies")
    public List<Company> getUserCompanies(@RequestParam long id) throws NotFoundCompanyException {
        List<Company> companies = new ArrayList<>();
        Iterable<BigInteger> idList =  registeredUserService.findUserCompanies(id);
        idList.forEach(element -> {
            try {
                companies.add(companyService.findById(element.longValue()));
            } catch (NotFoundCompanyException e) {
                //Cannot be thrown
            }
        });
        return companies;
    }
}
