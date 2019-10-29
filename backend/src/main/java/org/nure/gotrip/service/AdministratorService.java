package org.nure.gotrip.service;

import org.nure.gotrip.exception.NotFoundAdministratorException;
import org.nure.gotrip.exception.NotUniqueAdministratorException;
import org.nure.gotrip.model.Administrator;

public interface AdministratorService {

	Administrator addAdministrator(Administrator administrator) throws NotUniqueAdministratorException;

	Administrator getById(long id) throws NotFoundAdministratorException;
}