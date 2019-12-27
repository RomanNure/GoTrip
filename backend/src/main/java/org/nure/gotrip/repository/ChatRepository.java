package org.nure.gotrip.repository;

import org.nure.gotrip.model.Chat;
import org.nure.gotrip.model.RegisteredUser;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface ChatRepository extends CrudRepository<Chat, Long> {

    Optional<Chat> findByFirstUserAndSecondUser(RegisteredUser user1, RegisteredUser user2);
}
