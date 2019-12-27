package org.nure.gotrip.controller.custom;

import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.ForbiddenException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.BooleanDto;
import org.nure.gotrip.dto.GuideInvitationDto;
import org.nure.gotrip.dto.GuidingRefusingDto;
import org.nure.gotrip.exception.NotFoundTourException;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.model.notification.*;
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
@RequestMapping("/custom/guide/invitation")
public class CustomGuideInvitationController {

    private NotificationService notificationService;
    private GuideService guideService;
    private TourService tourService;

    @Autowired
    public CustomGuideInvitationController(NotificationService notificationService, GuideService guideService, TourService tourService) {
        this.notificationService = notificationService;
        this.guideService = guideService;
        this.tourService = tourService;
    }

    @PostMapping("/fromguide")
    public ResponseEntity proposeGuide(@RequestBody GuideInvitationDto dto) {
        Tour tour;
        try {
            tour = tourService.findById(dto.getTourId());
        } catch (NotFoundTourException e) {
            throw new NotFoundException(e.getMessage());
        }
        if(!tour.isCustom()){
            throw new ForbiddenException("Tour is not custom");
        }
        Guide guide = getGuide(dto.getGuideId());
        if (isAbleForGuiding(tour, guide)) {
            CustomGuidePropositionNotificationData data = getGuidePropositionNotificationData(dto);
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
            tour.setGuideSalary(dto.getSum());
            try {
                tourService.update(tour);
            } catch (NotFoundTourException e) {
                throw new NotFoundException(e.getMessage());
            }
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

    private CustomGuidePropositionNotificationData getGuidePropositionNotificationData(GuideInvitationDto dto) {
        CustomGuidePropositionNotificationData data = new CustomGuidePropositionNotificationData();
        data.setTourId(dto.getTourId());
        data.setGuideId(dto.getGuideId());
        data.setSum(dto.getSum());
        return data;
    }

    private ResponseEntity getGuidePropositionNotificationResponseEntity(CustomGuidePropositionNotificationData data, Tour tour) {
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

    private void sendAcceptNotificationAdmin(Guide guide, Administrator administrator, Tour tour) {
        PlainTextNotificationData data = new PlainTextNotificationData();
        data.setText(format("Admin %s accepted to let you guide a tour %s.", administrator.getRegisteredUser().getLogin(), tour.getName()));
        Notification notification = new Notification(data);
        notification.setUser(guide.getRegisteredUser());
        notification.setTopic("Admin accepted");
        notificationService.add(notification);
    }

    @PostMapping("/accept/admin")
    public ResponseEntity acceptAdmin(@RequestBody GuideInvitationDto dto) {
        Tour tour = getTour(dto.getTourId());
        Guide guide = getGuide(dto.getGuideId());
        if (isAbleForGuiding(tour, guide)) {
            tour = tourService.setGuide(tour, guide);
            tour.setGuideSalary(dto.getSum());
            try {
                tourService.update(tour);
            } catch (NotFoundTourException e) {
                throw new NotFoundException(e.getMessage());
            }
            sendAcceptNotificationAdmin(guide, tour.getAdministrator(), tour);
            return new ResponseEntity<>(tour, HttpStatus.OK);
        }
        throw new ConflictException("Cannot be a guide for this tour");
    }

    private Notification sendRefuseNotificationAdmin(Guide guide, Administrator administrator, Tour tour) {
        PlainTextNotificationData data = new PlainTextNotificationData();
        data.setText(format("Admin %s refused to let you guide a tour %s.", administrator.getRegisteredUser().getLogin(), tour.getName()));
        Notification notification = new Notification(data);
        notification.setUser(guide.getRegisteredUser());
        notification.setTopic("Admin refused");
        return notificationService.add(notification);
    }

    @PostMapping("/refuse/admin")
    public ResponseEntity refuseAdmin(@RequestBody GuidingRefusingDto dto) {
        Tour tour = getTour(dto.getTourId());
        Guide guide = getGuide(dto.getGuideId());
        Administrator admin = tour.getAdministrator();
        Notification notification = sendRefuseNotificationAdmin(guide, admin, tour);
        return new ResponseEntity<>(notification, HttpStatus.OK);
    }
}