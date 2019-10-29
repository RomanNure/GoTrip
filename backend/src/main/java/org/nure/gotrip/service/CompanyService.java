package org.nure.gotrip.service;

import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueCompanyException;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.model.Company;

import java.util.List;

public interface CompanyService {

	Company add(Company company) throws NotUniqueCompanyException, NotFoundUserException;

	Company update(Company company);

	void remove(long id);

	Company findByName(String name) throws NotFoundCompanyException;

	Company findById(long id) throws NotFoundCompanyException;

    Company findByAdmin(long administratorId) throws NotFoundCompanyException;

    List<Company> findByOwner(long id) throws NotFoundCompanyException;
}