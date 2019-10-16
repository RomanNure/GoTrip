package org.nure.gotrip.service.impl;

import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueCompanyException;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.repository.CompanyRepository;
import org.nure.gotrip.repository.RegisteredUserRepository;
import org.nure.gotrip.service.CompanyService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

@Service
public class CompanyServiceImpl implements CompanyService {

	private CompanyRepository companyRepository;

	@Autowired
	public CompanyServiceImpl(CompanyRepository companyRepository) {
		this.companyRepository = companyRepository;
	}

	@Override
	public Company add(Company company) throws NotUniqueCompanyException {
        try{
			return companyRepository.save(company);
		} catch (DataIntegrityViolationException ex) {
			throw new NotUniqueCompanyException("The database contains a company with this name");
		}
	}

	@Override
	public Company update(Company company) {
		return companyRepository.save(company);
	}

	@Override
	public void remove(long id) {
		companyRepository.deleteById(id);
	}

	@Override
	public Company findByName(String name) throws NotFoundCompanyException {
		return companyRepository.findByName(name).orElseThrow(() -> new NotFoundCompanyException("Company with such name does not exist"));
	}

	@Override
	public Company findById(long id) throws NotFoundCompanyException {
		return companyRepository.findById(id).orElseThrow(() -> new NotFoundCompanyException("Company with such id does not exist"));
	}

}
