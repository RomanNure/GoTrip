package org.nure.gotrip.repository;

import org.nure.gotrip.model.RegisteredUser;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface RegisteredUserRepository extends CrudRepository<RegisteredUser, Long> {
}