package org.nure.gotrip.service;

import org.nure.gotrip.exception.NotFoundNotificationException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.notification.Notification;

public interface NotificationService {

    Notification add(Notification notification);

    Iterable<Notification> getByUser(RegisteredUser user);

    Notification getById(String id) throws NotFoundNotificationException;

    Notification update(Notification notification);

    void delete(Notification notification);
}
