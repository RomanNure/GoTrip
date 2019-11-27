package org.nure.gotrip.service.impl;

import org.nure.gotrip.exception.NotFoundNotificationException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.notification.Notification;
import org.nure.gotrip.repository.NotificationRepository;
import org.nure.gotrip.service.NotificationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.UUID;

@Service
public class NotificationServiceImpl implements NotificationService {

    private NotificationRepository notificationRepository;

    @Autowired
    public NotificationServiceImpl(NotificationRepository notificationRepository) {
        this.notificationRepository = notificationRepository;
    }

    @Override
    public Notification add(Notification notification) {
        notification.setId(UUID.randomUUID().toString());
        return notificationRepository.save(notification);
    }

    @Override
    public Iterable<Notification> getByUser(RegisteredUser user) {
        return notificationRepository.findByUser(user);
    }

    @Override
    public Notification getById(String id) throws NotFoundNotificationException {
        return notificationRepository.findById(id).orElseThrow(()->new NotFoundNotificationException("Notification not found"));
    }

    @Override
    public Notification update(Notification notification) {
        return notificationRepository.save(notification);
    }

    @Override
    public void delete(Notification notification) {
        notificationRepository.delete(notification);
    }
}
