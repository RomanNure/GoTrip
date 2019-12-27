package org.nure.gotrip.controller;

import com.liqpay.LiqPay;
import org.nure.gotrip.controller.response.ForbiddenException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.RateDto;
import org.nure.gotrip.exception.NotFoundParticipatingException;
import org.nure.gotrip.exception.NotFoundTourException;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.Participating;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.service.ParticipatingService;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.service.TourService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

import java.util.HashMap;
import java.util.Map;

@Controller
@RequestMapping("/rate")
public class RateController {

    private static final Logger LOGGER = LoggerFactory.getLogger(RateController.class);

    private static final String PUBLIC_KEY = "sandbox_i74310151520";
    private static final String PRIVATE_KEY = "sandbox_kgwzzF9TsmUOIJmQQeQyM4G4yrxfGJxVq64k8hLn";

    private ParticipatingService participatingService;
    private TourService tourService;
    private RegisteredUserService registeredUserService;

    @Autowired
    public RateController(ParticipatingService participatingService, TourService tourService, RegisteredUserService registeredUserService) {
        this.participatingService = participatingService;
        this.tourService = tourService;
        this.registeredUserService = registeredUserService;
    }

    @PostMapping(value = "/tour", produces = "application/json")
    public ResponseEntity rateTour(@RequestBody RateDto dto) throws Exception {
        Tour tour;
        RegisteredUser user;

        try {
            tour = tourService.findById(dto.getTourId());
            user = registeredUserService.findById(dto.getUserId());


            Participating participating;
            try {
                participating = participatingService.getByTourAndUser(tour, user);
            } catch (NotFoundParticipatingException e) {
                throw new NotFoundException(e.getMessage());
            }
            if (!participating.isParticipated()) {
                throw new ForbiddenException("You haven't passed ticket control");
            }
            participating.setGuideRate(dto.getGuideRate());
            participating.setTourRate(dto.getTourRate());
            participating.setFinished(true);
            participating = participatingService.update(participating);

            if (!tour.isEnded()) {
                finishTour(tour);
            }

            return new ResponseEntity<>(participating, HttpStatus.OK);
        } catch (NotFoundTourException e) {
            throw new NotFoundException(e.getMessage());
        } catch (NotFoundUserException e) {
            throw new NotFoundException(e.getMessage());
        }catch(Exception e){
            throw e;
        }
    }

    private void finishTour(Tour tour) throws Exception {
        long voted = tour.getParticipatingList().stream()
                .filter(Participating::isFinished)
                .count();
        if (voted / tour.getParticipatingList().size() >= 0.5) {
            tour.setEnded(true);
            tourService.update(tour);
            tour.getParticipatingList().forEach(participating -> {
                if(participating.getOrderId() != null && !participating.getOrderId().equals("")) {
                    try {
                        confirmPayment(participating);
                    } catch (Exception e) {
                        LOGGER.error("Unknown exception", e);
                    }
                }
            });

            HashMap<String, String> params = new HashMap<>();
            params.put("action", "p2pcredit");
            params.put("version", "3");
            params.put("amount", String.valueOf(tour.getGuideSalary()));
            params.put("currency", "USD");
            params.put("description", "For guiding");
            params.put("order_id", tour.getGuide().getId() + "_" + tour.getId());
            params.put("receiver_card", tour.getGuide().getCardNumber());

            LiqPay liqpay = new LiqPay(PUBLIC_KEY, PRIVATE_KEY);
            Map<String, Object> res = liqpay.api("request", params);
            LOGGER.info(String.format("Payment confirmed for guiding tour %d. Status: %s",
                    tour.getId(),
                    res.get("status")));
        }
    }

    private void confirmPayment(Participating participating) throws Exception {
        HashMap<String, String> params = new HashMap<>();
        params.put("action", "paylc_confirm");
        params.put("version", "3");
        params.put("order_id", participating.getOrderId());
        params.put("confirm", participating.isParticipated() ? "yes" : "no");

        LiqPay liqpay = new LiqPay(PUBLIC_KEY, PRIVATE_KEY);
        Map<String, Object> res = liqpay.api("request", params);
        LOGGER.info(String.format("Payment finished for participating %d. Status: %s",
                participating.getId(),
                res.get("status")));
    }
}
