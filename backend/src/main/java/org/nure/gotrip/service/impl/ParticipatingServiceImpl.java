package org.nure.gotrip.service.impl;

import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.model.Participating;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.repository.ParticipatingRepository;
import org.nure.gotrip.repository.RegisteredUserRepository;
import org.nure.gotrip.repository.TourRepository;
import org.nure.gotrip.service.ParticipatingService;
import org.nure.gotrip.service.TourService;
import org.nure.gotrip.util.Encoder;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

import static java.lang.String.format;

@Service
public class ParticipatingServiceImpl implements ParticipatingService {

    private ParticipatingRepository participatingRepository;
    private TourService tourService;
    private TourRepository tourRepository;
    private RegisteredUserRepository registeredUserRepository;
    private Encoder encoder;

    @Autowired
    public ParticipatingServiceImpl(ParticipatingRepository participatingRepository, TourService tourService, TourRepository tourRepository, RegisteredUserRepository registeredUserRepository, Encoder encoder) {
        this.participatingRepository = participatingRepository;
        this.tourService = tourService;
        this.tourRepository = tourRepository;
        this.registeredUserRepository = registeredUserRepository;
        this.encoder = encoder;
    }

    @Override
    public boolean isAbleToParticipate(long userId, long tourId) {
        Tour tour = tourRepository.findById(tourId).orElseThrow(()->new NotFoundException("Tour Not found"));
        List<Tour> userTours = tourService.getByUser(userId);
        return userTours.stream().allMatch(userTour -> isCompatible(userTour, tour));
    }

    @Override
    public void participate(long userId, long tourId) {
        if(!isAbleToParticipate(userId, tourId)){
            throw new ConflictException("Cannot participate at that time");
        }
        Participating participating = new Participating();
        participating.setUser(registeredUserRepository.findById(userId).orElseThrow(()->new NotFoundException("User not found")));
        participating.setTour(tourRepository.findById(tourId).orElseThrow(()->new NotFoundException("Tour not found")));
        String compilation = format("%d %d", userId, tourId);
        String hash = encoder.encode(compilation);
        participating.setHash(hash);
        participatingRepository.save(participating);
    }

    private boolean isCompatible(Tour tour1, Tour tour2){
        return tour1.getFinishDateTime().compareTo(tour2.getStartDateTime()) < 0 ||
                tour1.getStartDateTime().compareTo(tour2.getFinishDateTime()) > 0;
    }
}


