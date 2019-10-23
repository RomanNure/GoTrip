package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.exception.ValidationException;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.service.CompanyService;
import org.nure.gotrip.util.validation.CompanyUpdateValidator;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/company")
public class CompanyUpdateController {

	private static final Logger logger = LoggerFactory.getLogger(CompanyUpdateController.class);

	private CompanyService companyService;
	private CompanyUpdateValidator companyUpdateValidator;


	@Autowired
	public CompanyUpdateController(CompanyService companyService, CompanyUpdateValidator companyUpdateValidator) {
		this.companyService = companyService;
		this.companyUpdateValidator = companyUpdateValidator;
	}

	@PostMapping(value = "/update", produces = "application/json")
	public Company updateUserData(@RequestBody Company updatedCompany) {
		try {
			companyUpdateValidator.updateCompanyValid(updatedCompany);
		} catch (ValidationException e) {
			logger.info("Invalid request data");
			throw new BadRequestException(e.getMessage());
		}
		long companyId = updatedCompany.getId();
		Company oldCompany = getCompany(companyId);
		changeCompanyInformation(oldCompany, updatedCompany);
		companyService.update(oldCompany);
		return oldCompany;
	}

	private Company getCompany(long userId) {
		try {
			return companyService.findById(userId);
		} catch (NotFoundCompanyException e) {
			logger.info("User with invalid id tried to update");
			throw new BadRequestException("Invalid id");
		}
	}

	private void changeCompanyInformation(Company oldCompany, Company updatedCompany) {
		oldCompany.setName(updatedCompany.getName());
		oldCompany.setEmail(updatedCompany.getEmail());
		oldCompany.setImageLink(updatedCompany.getImageLink());
		oldCompany.setDescription(updatedCompany.getDescription());
	}

}
