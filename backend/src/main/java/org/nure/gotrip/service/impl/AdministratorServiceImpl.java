package org.nure.gotrip.service.impl;

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
        }catch (DataIntegrityViolationException e){
            throw new NotUniqueAdministratorException(e.getMessage());
        }
    }
}
