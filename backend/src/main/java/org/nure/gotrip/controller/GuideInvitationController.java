package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.BooleanDto;
import org.nure.gotrip.dto.GuideInvitationDto;
import org.nure.gotrip.dto.GuidingRefusingDto;
import org.nure.gotrip.exception.NotFoundTourException;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.model.notification.GuidePropositionNotificationData;
import org.nure.gotrip.model.notification.Notification;
import org.nure.gotrip.model.notification.OfferGuidingNotificationData;
import org.nure.gotrip.model.notification.PlainTextNotificationData;
import org.nure.gotrip.service.GuideService;
import org.nure.gotrip.service.NotificationService;
import org.nure.gotrip.service.TourService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import java.util.Date;
import java.util.Objects;

import static java.lang.String.format;

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
    public ResponseEntity inviteGuide(@RequestBody GuideInvitationDto dto) {
        Tour tour;
        try {
            tour = tourService.findById(dto.getTourId());
        } catch (NotFoundTourException e) {
            throw new NotFoundException(e.getMessage());
        }
        OfferGuidingNotificationData data = getOfferGuidingNotificationData(dto, tour);
        Guide guide = getGuide(dto.getGuideId());
        if (isAbleForGuiding(tour, guide)) {
            return getNotificationResponseEntity(data, guide);
        }
        throw new ConflictException("guide cannot guiding that tour");
    }

    @PostMapping("/fromguide")
    public ResponseEntity proposeGuide(@RequestBody GuideInvitationDto dto) {
        Tour tour;
        try {
            tour = tourService.findById(dto.getTourId());
        } catch (NotFoundTourException e) {
            throw new NotFoundException(e.getMessage());
        }
        Guide guide = getGuide(dto.getGuideId());
        if (isAbleForGuiding(tour, guide)) {
            GuidePropositionNotificationData data = getGuidePropositionNotificationData(dto);
            return getGuidePropositionNotificationResponseEntity(data, tour);
        }
        throw new ConflictException("guide cannot guiding that tour");
    }

    @GetMapping("/able")
    public ResponseEntity isAbleForGuiding(@RequestParam long tourId, @RequestParam long guideId) {
        Tour tour = getTour(tourId);
        Guide guide = getGuide(guideId);
        boolean result = isAbleForGuiding(tour, guide);
        return new ResponseEntity<>(new BooleanDto(result), HttpStatus.OK);
    }

    @PostMapping("/accept")
    public ResponseEntity acceptGuiding(@RequestBody GuideInvitationDto dto) {
        Tour tour = getTour(dto.getTourId());
        Guide guide = getGuide(dto.getGuideId());
        if (isAbleForGuiding(tour, guide)) {
            tour = tourService.setGuide(tour, guide);
            sendAcceptNotification(guide, tour.getAdministrator(), tour);
            return new ResponseEntity<>(tour, HttpStatus.OK);
        }
        throw new ConflictException("Cannot be a guide for this tour");
    }

    @PostMapping("/refuse")
    public ResponseEntity refuseGuiding(@RequestBody GuidingRefusingDto dto) {
        Tour tour = getTour(dto.getTourId());
        Guide guide = getGuide(dto.getGuideId());
        Administrator admin = tour.getAdministrator();
        Notification notification = sendRefusalNotification(guide, admin, tour);
        return new ResponseEntity<>(notification, HttpStatus.OK);
    }

    private boolean isAbleForGuiding(Tour tour, Guide guide) {
        Date nowDate = new Date();
        boolean cond1 = tour.getFinishDateTime().after(nowDate);
        boolean cond2 = tour.getGuide() != null && Objects.equals(tour.getGuide().getId(), guide.getId());
        boolean cond3 = Objects.equals(tour.getAdministrator().getRegisteredUser().getId(), guide.getRegisteredUser().getId());
        boolean cond4 = tourService.checkGuideOnToursBetweenDates(guide.getId(), nowDate, tour.getStartDateTime(), tour.getFinishDateTime());
        return cond1 && !cond2 && !cond3 && !cond4;
    }

    private Tour getTour(long tourId) {
        try {
            return tourService.findById(tourId);
        } catch (NotFoundTourException e) {
            throw new NotFoundException(e.getMessage());
        }
    }

    private Guide getGuide(long id) {
        return guideService.getById(id).orElseThrow(() -> new NotFoundException("Guide not found"));
    }

    private OfferGuidingNotificationData getOfferGuidingNotificationData(GuideInvitationDto dto, Tour tour) {
        OfferGuidingNotificationData data = new OfferGuidingNotificationData();
        data.setTourId(dto.getTourId());
        data.setGuideId(dto.getGuideId());
        data.setCompanyId(tour.getAdministrator().getCompany().getId());
        data.setSum(dto.getSum());
        return data;
    }

    private GuidePropositionNotificationData getGuidePropositionNotificationData(GuideInvitationDto dto) {
        GuidePropositionNotificationData data = new GuidePropositionNotificationData();
        data.setTourId(dto.getTourId());
        data.setGuideId(dto.getGuideId());
        data.setSum(dto.getSum());
        return data;
    }

    private ResponseEntity getNotificationResponseEntity(OfferGuidingNotificationData data, Guide guide) {
        Notification notification = new Notification(data);
        notification.setTopic("Offer received");
        notification.setUser(guide.getRegisteredUser());
        notification = notificationService.add(notification);
        return new ResponseEntity<>(notification, HttpStatus.OK);
    }

    private ResponseEntity getGuidePropositionNotificationResponseEntity(GuidePropositionNotificationData data, Tour tour) {
        Notification notification = new Notification(data);
        notification.setTopic("Proposition received");
        notification.setUser(tour.getAdministrator().getRegisteredUser());
        notification = notificationService.add(notification);
        return new ResponseEntity<>(notification, HttpStatus.OK);
    }

    private Notification sendRefusalNotification(Guide guide, Administrator administrator, Tour tour) {
        PlainTextNotificationData data = new PlainTextNotificationData();
        data.setText(format("Guide %s refused to be guide into tour %s.", guide.getRegisteredUser().getLogin(), tour.getName()));
        Notification notification = new Notification(data);
        notification.setUser(administrator.getRegisteredUser());
        notification.setTopic("Guide refused to participate into the tour");
        return notificationService.add(notification);
    }

    private void sendAcceptNotification(Guide guide, Administrator administrator, Tour tour) {
        PlainTextNotificationData data = new PlainTextNotificationData();
        data.setText(format("Guide %s accepted to be guide into tour %s.", guide.getRegisteredUser().getLogin(), tour.getName()));
        Notification notification = new Notification(data);
        notification.setUser(administrator.getRegisteredUser());
        notification.setTopic("Guide accepted to participate into the tour");
        notificationService.add(notification);
    }
}