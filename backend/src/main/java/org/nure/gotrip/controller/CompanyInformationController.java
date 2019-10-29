package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.service.CompanyService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/company")
public class CompanyInformationController {

	private CompanyService companyService;

	@Autowired
	public CompanyInformationController(CompanyService companyService) {
		this.companyService = companyService;
    }

	@GetMapping("/get")
	public Company getCompanyById(@RequestParam long id) {
		try {
			return companyService.findById(id);
		} catch (NotFoundCompanyException e) {
			throw new NotFoundException("Company did not find");
		}
	}

	@GetMapping("/get/admin")
    public ResponseEntity<Company> getByAdminId(@RequestParam long id){
	    try {
            Company company = companyService.findByAdmin(id);
            return new ResponseEntity<>(company, HttpStatus.OK);
        } catch (NotFoundCompanyException e) {
            throw new NotFoundException(e.getMessage());
        }
    }
}