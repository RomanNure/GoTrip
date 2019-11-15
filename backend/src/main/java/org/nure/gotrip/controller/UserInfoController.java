package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.RegisteredUserDto;
import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.service.CompanyService;
import org.nure.gotrip.service.GuideService;
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
import java.util.Optional;

@RestController
public class UserInfoController {

	private static final Logger logger = LoggerFactory.getLogger(UserInfoController.class);

	private RegisteredUserService registeredUserService;
	private CompanyService companyService;
	private GuideService guideService;

	@Autowired
	public UserInfoController(RegisteredUserService registeredUserService, CompanyService companyService, GuideService guideService) {
		this.registeredUserService = registeredUserService;
		this.companyService = companyService;
		this.guideService = guideService;
	}

	@GetMapping("/user/get")
	public RegisteredUserDto getUserInfo(@RequestParam long id) {
		try {
			return getUserInfoHandler(id);
		} catch (NotFoundUserException e) {
			logger.info(e.getMessage());
			throw new NotFoundException(e.getMessage());
		}
	}

	private RegisteredUserDto getUserInfoHandler(long id) throws NotFoundUserException {
		RegisteredUserDto registeredUserDto = new RegisteredUserDto();
		registeredUserDto.setRegisteredUser(registeredUserService.findById(id));
		Optional<Guide> guideOptional = guideService.getByRegisteredUserId(id);
		guideOptional.ifPresent(registeredUserDto::setGuide);
		registeredUserDto.setCompanies(registeredUserService.findUserCompanies(id));
		registeredUserDto.setAdministrators(registeredUserService.findAdministratorsByRegisteredUserId(id));
		return registeredUserDto;
	}

	@GetMapping("/user/get/companies")
	public List<Company> getUserCompanies(@RequestParam long id) {
		return getListOfCompanies(id);
	}

	private List<Company> getListOfCompanies(long id) {
		List<Company> companies = new ArrayList<>();
		Iterable<BigInteger> idList = registeredUserService.findUserCompanies(id);
		idList.forEach(element -> addCompanyToList(companies, element.longValue()));
		return companies;
	}

	private void addCompanyToList(List<Company> companies, long idCompany) {
		try {
			companies.add(companyService.findById(idCompany));
		} catch (NotFoundCompanyException e) {
			throw new NotFoundException(e.getMessage());
		}
	}

}