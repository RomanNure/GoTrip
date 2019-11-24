package org.nure.gotrip.service.impl;

import org.nure.gotrip.controller.response.ConflictException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.PreparingDto;
import org.nure.gotrip.model.Participating;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.repository.ParticipatingPreparationRepository;
import org.nure.gotrip.repository.ParticipatingRepository;
import org.nure.gotrip.repository.RegisteredUserRepository;
import org.nure.gotrip.repository.TourRepository;
import org.nure.gotrip.service.ParticipatingService;
import org.nure.gotrip.service.TourService;
import org.nure.gotrip.util.Encoder;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Date;
import java.util.List;
import java.util.Objects;
import java.util.Optional;

import static java.lang.String.format;

@Service
public class ParticipatingServiceImpl implements ParticipatingService {

	private ParticipatingPreparationRepository preparationRepository;
	private ParticipatingRepository participatingRepository;
	private TourService tourService;
	private TourRepository tourRepository;
	private RegisteredUserRepository registeredUserRepository;
	private Encoder encoder;

	@Autowired
	public ParticipatingServiceImpl(ParticipatingPreparationRepository preparationRepository, ParticipatingRepository participatingRepository, TourService tourService, TourRepository tourRepository, RegisteredUserRepository registeredUserRepository, Encoder encoder) {
		this.preparationRepository = preparationRepository;
		this.participatingRepository = participatingRepository;
		this.tourService = tourService;
		this.tourRepository = tourRepository;
		this.registeredUserRepository = registeredUserRepository;
		this.encoder = encoder;
	}

	@Override
	public boolean isAbleToParticipate(long userId, long tourId) {
		Tour tour = tourRepository.findById(tourId).orElseThrow(() -> new NotFoundException("Tour Not found"));
		if (tour.getGuide() == null || tour.getMaxParticipants() == tour.getParticipatingList().size()) {
			return false;
		}
		List<Tour> userTours = tourService.getByUser(userId);
		return userTours.stream().allMatch(userTour -> isCompatible(userTour, tour));
	}

    @Override
    public Participating participate(PreparingDto dto) {
        if(!isAbleToParticipate(dto.getUserId(), dto.getTourId())){
            throw new ConflictException("Cannot participate at that time");
        }
        Date nowDate = new Date();
        Participating participating = new Participating();
        RegisteredUser user = registeredUserRepository.findById(dto.getUserId()).orElseThrow(()->new NotFoundException("User not found"));
        Tour tour = tourRepository.findById(dto.getTourId()).orElseThrow(()->new NotFoundException("Tour not found"));
        if(Objects.equals(user.getId(),tour.getGuide().getRegisteredUser().getId())&&
                !checkUserOnGuidBetweenDates(dto.getUserId(), nowDate, tour.getStartDateTime(), tour.getFinishDateTime()) &&
                Objects.equals(tour.getAdministrator().getRegisteredUser().getId(), user.getId())) {
            throw new ConflictException("You cannot registered on the tour because you're the guide");
        }
        participating.setUser(user);
        participating.setTour(tour);

        String compilation = format("%d %d", dto.getUserId(), dto.getTourId());
        String hash = encoder.encode(compilation);
        participating.setHash(hash);
        participating.setOrderId(dto.getOrderId());
        participating = participatingRepository.save(participating);
        return participating;
    }

	@Override
	public boolean prepare(PreparingDto dto) {
		preparationRepository.add(dto);
		return true;
	}

	@Override
	public PreparingDto confirm(String orderId) {
		return preparationRepository.remove(orderId);
	}

    @Override
    public String getStatus(String orderId) {
        if(preparationRepository.get(orderId) != null){
            return "pending";
        }
        Optional<Participating> optional = participatingRepository.findByOrderId(orderId);
        return optional.isPresent() ? "success" : "failed";
    }

	private boolean isCompatible(Tour tour1, Tour tour2) {
		return tour1.getFinishDateTime().compareTo(tour2.getStartDateTime()) < 0 ||
				tour1.getStartDateTime().compareTo(tour2.getFinishDateTime()) > 0;
	}

	private boolean checkUserOnGuidBetweenDates(long userId, Date date, Date startDate, Date finishDate) {
		List<Tour> tours = participatingRepository.findTours(userId, date, startDate, finishDate);
		return tours.isEmpty();
	}
}