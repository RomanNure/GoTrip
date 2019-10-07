package org.nure.gotrip.serviсe;

import org.nure.gotrip.model.RegisteredUser;

public interface RegisteredUserService {

    RegisteredUser add(RegisteredUser user);

    RegisteredUser update(RegisteredUser user);

    RegisteredUser findById(long id);

    Iterable<RegisteredUser> findAll();
}
