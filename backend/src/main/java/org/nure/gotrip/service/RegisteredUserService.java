package org.nure.gotrip.service;

import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueUserException;
import org.nure.gotrip.model.RegisteredUser;

import java.math.BigInteger;

public interface RegisteredUserService {

	RegisteredUser add(RegisteredUser user) throws NotUniqueUserException;

	RegisteredUser update(RegisteredUser user);

	RegisteredUser findById(long id) throws NotFoundUserException;

	RegisteredUser findByEmail(String email) throws NotFoundUserException;

	Iterable<RegisteredUser> findAll();

	boolean checkPassword(RegisteredUser user, String password);

	RegisteredUser findByLogin(String login) throws NotFoundUserException;

	Iterable<BigInteger> findUserCompanies(Long userId);

	RegisteredUser findByAdministrator(long administratorId);

	Iterable<BigInteger> findAdministratorsByRegisteredUserId(long id);
}
