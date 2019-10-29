package org.nure.gotrip.service.impl;

import org.nure.gotrip.exception.NotFoundCompanyException;
import org.nure.gotrip.exception.NotUniqueCompanyException;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.repository.CompanyRepository;
import org.nure.gotrip.service.CompanyService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.math.BigInteger;
import java.util.ArrayList;
import java.util.List;

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

	public Company findByAdmin(long administratorId) throws NotFoundCompanyException {
	    BigInteger companyId = companyRepository.findByAdministrator(administratorId);
	    if(companyId == null){
	        throw new NotFoundCompanyException("Company was not found");
        }
	    return companyRepository.findById(companyId.longValue()).orElseThrow(()->new NotFoundCompanyException("Company was not found"));
    }

    @Override
    public List<Company> findByOwner(long id) throws NotFoundCompanyException {
        Iterable<BigInteger> companyIds = companyRepository.findByOwner(id);
        List<Company> result = new ArrayList<>();
        for(BigInteger companyId : companyIds){
            result.add(companyRepository.findById(companyId.longValue()).orElseThrow(()-> new NotFoundCompanyException("Company not found")));
        }
        return result;
    }
}
