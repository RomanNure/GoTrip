package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.AbilityParticipatingDto;
import org.nure.gotrip.dto.GuideInvitationDto;
import org.nure.gotrip.exception.NotFoundTourException;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.model.notification.Notification;
import org.nure.gotrip.model.notification.OfferGuidingNotificationData;
import org.nure.gotrip.service.GuideService;
import org.nure.gotrip.service.NotificationService;
import org.nure.gotrip.service.TourService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/guide/invitation")
public class GuideInvitationController {

    private NotificationService notificationService;
    private GuideService guideService;
    private TourService tourService;

    @Autowired
    public GuideInvitationController(NotificationService notificationService, GuideService guideService, TourService tourService) {
        this.notificationService = notificationService;
        this.guideService = guideService;
        this.tourService = tourService;
    }

    @PostMapping("/fromadmin")
    public ResponseEntity inviteGuide(@RequestBody GuideInvitationDto dto){
        OfferGuidingNotificationData data = new OfferGuidingNotificationData();
        data.setTourId(dto.getTourId());
        data.setGuideId(dto.getGuideId());
        data.setPrice(dto.getSum());
        Guide guide = guideService.getById(dto.getGuideId()).orElseThrow(()->new NotFoundException("Guide not found"));
        Notification notification = new Notification(data);
        notification.setTopic("Offer received");
        notification.setUser(guide.getRegisteredUser());
        notification = notificationService.add(notification);
        return new ResponseEntity<>(notification, HttpStatus.OK);
    }

    @GetMapping("/able")
    public ResponseEntity isAbleForGuiding(@RequestBody GuideInvitationDto dto){
        Tour tour = getTour(dto.getTourId());
        Guide guide = guideService.getById(dto.getGuideId()).orElseThrow(()->new NotFoundException("Guide not found"));
        return new ResponseEntity<>(new AbilityParticipatingDto(isAbleForGuiding(tour, guide)), HttpStatus.OK);
    }

    @PostMapping("/accept")
    public ResponseEntity acceptGuiding(@RequestBody GuideInvitationDto dto){
        Tour tour = getTour(dto.getTourId());
        Guide guide = guideService.getById(dto.getGuideId()).orElseThrow(()->new NotFoundException("Guide not found"));
        if(isAbleForGuiding(tour, guide)) {
            tour = tourService.setGuide(tour, guide);
            return new ResponseEntity<>(tour, HttpStatus.OK);
        }
        throw new ConflictException("Cannot be a guide for this tour");
    }

    private boolean isAbleForGuiding(Tour tour, Guide guide){
        //TODO add checking
        if(tour.getGuide() == null){
            return true;
        }
        return false;
    }

    private Tour getTour(long tourId){
        try {
            return tourService.findById(tourId);
        } catch (NotFoundTourException e) {
            throw new NotFoundException(e.getMessage());
        }
    }
}
