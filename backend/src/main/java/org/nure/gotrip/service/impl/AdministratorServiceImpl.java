package org.nure.gotrip.service.impl;

import org.nure.gotrip.exception.NotFoundAdministratorException;
import org.nure.gotrip.exception.NotUniqueAdministratorException;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.repository.AdministratorRepository;
import org.nure.gotrip.service.AdministratorService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

@Service
public class AdministratorServiceImpl implements AdministratorService {

	private AdministratorRepository administratorRepository;

	@Autowired
	public AdministratorServiceImpl(AdministratorRepository administratorRepository) {
		this.administratorRepository = administratorRepository;
	}

	@Override
	public Administrator addAdministrator(Administrator administrator) throws NotUniqueAdministratorException {
		try {
			return administratorRepository.save(administrator);
		} catch (DataIntegrityViolationException e) {
			throw new NotUniqueAdministratorException(e.getMessage());
		}
	}

	@Override
	public Administrator getById(long id) throws NotFoundAdministratorException {
		return administratorRepository.findById(id).orElseThrow(() -> new NotFoundAdministratorException("Company with such name does not exist"));
	}

	@Override
	public Administrator update(Administrator administrator) {
		return administratorRepository.save(administrator);
	}

	@Override
	public void remove(long id) throws NotFoundAdministratorException {
		Administrator admin = administratorRepository.findById(id)
				.orElseThrow(() -> new NotFoundAdministratorException("Administrator not found"));
		administratorRepository.delete(admin);
	}

}