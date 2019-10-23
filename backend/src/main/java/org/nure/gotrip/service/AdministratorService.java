package org.nure.gotrip.service;

import org.nure.gotrip.model.Administrator;
import org.springframework.web.bind.annotation.PostMapping;

public interface AdministratorService {

    Administrator addAdministrator(Administrator administrator);
}
