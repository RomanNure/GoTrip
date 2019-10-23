package org.nure.gotrip.service.impl;

import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.repository.AdministratorRepository;
import org.nure.gotrip.service.AdministratorService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class AdministratorServiceImpl implements AdministratorService {

    private AdministratorRepository administratorRepository;

    @Autowired
    public AdministratorServiceImpl(AdministratorRepository administratorRepository) {
        this.administratorRepository = administratorRepository;
    }

    @Override
    public Administrator addAdministrator(Administrator administrator){
        return administratorRepository.save(administrator);
    }
}
