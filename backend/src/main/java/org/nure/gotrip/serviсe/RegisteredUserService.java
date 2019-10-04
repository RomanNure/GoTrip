package org.nure.gotrip.serviсe;

import org.nure.gotrip.model.RegisteredUser;

public interface RegisteredUserService {

    void save(RegisteredUser user);

    RegisteredUser findById(long id);

    Iterable<RegisteredUser> findAll();
}
