package org.nure.gotrip.serviсe.impl;

import org.nure.gotrip.serviсe.RegisteredUserService;
import org.nure.gotrip.util.Encoder;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.repository.RegisteredUserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Date;

@Service
public class RegisteredUserServiceImpl implements RegisteredUserService {

    private RegisteredUserRepository registeredUserRepository;

    private Encoder encoder;

    @Autowired
    public RegisteredUserServiceImpl(RegisteredUserRepository registeredUserRepository, Encoder encoder) {
        this.registeredUserRepository = registeredUserRepository;
        this.encoder = encoder;
    }

    public RegisteredUser add(RegisteredUser user){
        user.setPassword(encoder.encode(user.getPassword()));
        user.setRegistrationDatetime(new Date(System.currentTimeMillis()));
        return registeredUserRepository.save(user);
    }

    public RegisteredUser update(RegisteredUser user) {
        return registeredUserRepository.save(user);
    }

    public RegisteredUser findById(long id) {
        return registeredUserRepository.findById(id).orElse(null);
    }

    public Iterable<RegisteredUser> findAll() {
        return registeredUserRepository.findAll();
    }
}
