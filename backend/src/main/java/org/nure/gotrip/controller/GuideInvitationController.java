package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.GuideInvitationDto;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.model.notification.Notification;
import org.nure.gotrip.model.notification.OfferGuidingNotificationData;
import org.nure.gotrip.service.GuideService;
import org.nure.gotrip.service.NotificationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/guide/invitation")
public class GuideInvitationController {

    private NotificationService notificationService;
    private GuideService guideService;

    @Autowired
    public GuideInvitationController(NotificationService notificationService, GuideService guideService) {
        this.notificationService = notificationService;
        this.guideService = guideService;
    }

    @PostMapping("/fromadmin")
    public ResponseEntity inviteGuide(@RequestBody GuideInvitationDto dto){
        OfferGuidingNotificationData data = new OfferGuidingNotificationData();
        data.setTourId(dto.getTourId());
        data.setGuideId(dto.getTourId());
        Guide guide = guideService.getById(dto.getGuideId()).orElseThrow(()->new NotFoundException("Guide not found"));
        Notification notification = new Notification(data);
        notification.setTopic("Offer received");
        notification.setUser(guide.getRegisteredUser());
        notification = notificationService.add(notification);
        return new ResponseEntity<>(notification, HttpStatus.OK);
    }
}
