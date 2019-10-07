package org.nure.gotrip.serviсe.impl;

import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueUserException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.repository.RegisteredUserRepository;
import org.nure.gotrip.serviсe.RegisteredUserService;
import org.nure.gotrip.util.Encoder;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DuplicateKeyException;
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

	public void add(RegisteredUser user) {
		try {
			user.setPassword(encoder.encode(user.getPassword()));
			user.setRegistrationDatetime(new Date(System.currentTimeMillis()));
			registeredUserRepository.save(user);
		} catch (DuplicateKeyException ex) {
			throw new NotUniqueUserException("The database contains a user with this login");
		}
	}

	public RegisteredUser update(RegisteredUser user) {
		return registeredUserRepository.save(user);
	}

	public RegisteredUser findById(long id) {
		return registeredUserRepository.findById(id).orElseThrow(() -> new NotFoundUserException("User did not find by id"));
	}

	public Iterable<RegisteredUser> findAll() {
		return registeredUserRepository.findAll();
	}
}
