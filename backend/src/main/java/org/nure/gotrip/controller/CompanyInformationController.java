package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.service.CompanyService;
import org.springframework.beans.factory.annotation.Autowired;
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

}