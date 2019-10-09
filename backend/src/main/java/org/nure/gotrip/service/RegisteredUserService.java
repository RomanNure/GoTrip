package org.nure.gotrip.service;

import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueUserException;
import org.nure.gotrip.model.RegisteredUser;

public interface RegisteredUserService {

	void add(RegisteredUser user) throws NotUniqueUserException;

	RegisteredUser update(RegisteredUser user);

	RegisteredUser findById(long id) throws NotFoundUserException;

	Iterable<RegisteredUser> findAll();

	boolean checkPassword(RegisteredUser user, String password);

	RegisteredUser findByLogin(String login) throws NotFoundUserException;
}
