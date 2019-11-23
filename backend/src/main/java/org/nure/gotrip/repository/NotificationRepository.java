package org.nure.gotrip.repository;

import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.notification.Notification;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface NotificationRepository extends CrudRepository<Notification, Long> {

    Iterable<Notification> findByUser(RegisteredUser user);
}
