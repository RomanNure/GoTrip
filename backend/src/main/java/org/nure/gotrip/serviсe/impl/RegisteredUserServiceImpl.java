package org.nure.gotrip.serviсe.impl;

import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.repository.RegisteredUserRepository;
import org.nure.gotrip.serviсe.RegisteredUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class RegisteredUserServiceImpl implements RegisteredUserService {

    private RegisteredUserRepository registeredUserRepository;

    @Autowired
    public RegisteredUserServiceImpl(RegisteredUserRepository registeredUserRepository) {
        this.registeredUserRepository = registeredUserRepository;
    }

    public void save(RegisteredUser user) {
        registeredUserRepository.save(user);
    }

    public RegisteredUser findById(long id) {
        return registeredUserRepository.findById(id).orElse(null);
    }

    public Iterable<RegisteredUser> findAll() {
        return registeredUserRepository.findAll();
    }
}
