package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.BooleanDto;
import org.nure.gotrip.dto.NotificationDto;
import org.nure.gotrip.exception.NotFoundNotificationException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.notification.Notification;
import org.nure.gotrip.service.NotificationService;
import org.nure.gotrip.service.RegisteredUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import java.util.Iterator;
import java.util.concurrent.atomic.AtomicBoolean;

@Controller
@RequestMapping("/notification")
public class NotificationController {

    private RegisteredUserService userService;
    private NotificationService notificationService;

    @Autowired
    public NotificationController(RegisteredUserService userService, NotificationService notificationService) {
        this.userService = userService;
        this.notificationService = notificationService;
    }

    @GetMapping("/new")
    public ResponseEntity getNewNotificationsExists(@RequestParam long id){
        try {
            RegisteredUser user = userService.findById(id);
            AtomicBoolean atomicBoolean = new AtomicBoolean(false);
            notificationService.getByUser(user).forEach(notification -> {
                if(!notification.isChecked()){
                    atomicBoolean.set(true);
                }
            });
            return new ResponseEntity<>(new BooleanDto(atomicBoolean.get()), HttpStatus.OK);
        } catch (NotFoundUserException e) {
            throw new NotFoundException("User not found");
        }
    }

    @GetMapping("/get/user")
    public ResponseEntity getUserNotifications(@RequestParam long id){
        try {
            RegisteredUser user = userService.findById(id);
            return new ResponseEntity<>(notificationService.getByUser(user), HttpStatus.OK);
        } catch (NotFoundUserException e) {
            throw new NotFoundException("User not found");
        }
    }

    @PostMapping("/check")
    public ResponseEntity checkNotification(@RequestBody NotificationDto dto){
        try {
            Notification notification = notificationService.getById(dto.getNotificationId());
            notification.setChecked(true);
            notification = notificationService.update(notification);
            return new ResponseEntity<>(notification, HttpStatus.OK);
        } catch (NotFoundNotificationException e) {
            throw new NotFoundException(e.getMessage());
        }
    }

    @PostMapping("/delete")
    public ResponseEntity deleteNotification(@RequestBody NotificationDto dto){
        try {
            Notification notification = notificationService.getById(dto.getNotificationId());
            notificationService.delete(notification);
            return new ResponseEntity<>(notification, HttpStatus.OK);
        } catch (NotFoundNotificationException e) {
            throw new NotFoundException(e.getMessage());
        }
    }
}
