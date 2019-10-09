package org.nure.gotrip.service.impl;

import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueUserException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.repository.RegisteredUserRepository;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.util.Encoder;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
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

	public void add(RegisteredUser user) throws NotUniqueUserException{
		try {
			user.setPassword(encoder.encode(user.getPassword()));
			user.setRegistrationDatetime(new Date(System.currentTimeMillis()));
			registeredUserRepository.save(user);
		} catch (DataIntegrityViolationException ex) {
			throw new NotUniqueUserException("The database contains a user with this login");
		}
	}

	public RegisteredUser update(RegisteredUser user) {
		return registeredUserRepository.save(user);
	}

	public RegisteredUser findById(long id) throws NotFoundUserException{
		return registeredUserRepository.findById(id).orElseThrow(() -> new NotFoundUserException("User did not find by id"));
	}

	public Iterable<RegisteredUser> findAll() {
		return registeredUserRepository.findAll();
	}

    @Override
    public boolean checkPassword(RegisteredUser user, String password) {
        String encodedPassword = encoder.encode(password);
        return user.getPassword().equals(encodedPassword);
    }

    @Override
    public RegisteredUser findByLogin(String login) throws NotFoundUserException {
        return registeredUserRepository.findByLogin(login).orElseThrow(()->new NotFoundUserException("User with such login doesn't exist"));
    }
}
