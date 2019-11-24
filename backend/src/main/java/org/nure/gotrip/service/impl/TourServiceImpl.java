package org.nure.gotrip.service.impl;

import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.FilterUnit;
import org.nure.gotrip.exception.NotFoundTourException;
import org.nure.gotrip.exception.NotUniqueTourException;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.model.Participating;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.model.TourPhoto;
import org.nure.gotrip.repository.TourJdbcRepository;
import org.nure.gotrip.repository.TourPhotoRepository;
import org.nure.gotrip.repository.TourRepository;
import org.nure.gotrip.service.TourService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.math.BigInteger;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class TourServiceImpl implements TourService {

	private static final Logger LOGGER = LoggerFactory.getLogger(TourServiceImpl.class);

	private TourRepository tourRepository;
	private TourPhotoRepository tourPhotoRepository;
	private TourJdbcRepository tourJdbcRepository;

	@Autowired
	public TourServiceImpl(TourRepository tourRepository, TourPhotoRepository tourPhotoRepository, TourJdbcRepository tourJdbcRepository) {
		this.tourRepository = tourRepository;
		this.tourPhotoRepository = tourPhotoRepository;
		this.tourJdbcRepository = tourJdbcRepository;
	}

	public Tour findById(long id) throws NotFoundTourException {
		return tourRepository.findById(id).orElseThrow(() -> new NotFoundTourException("Tour was Not Found"));
	}

	@Override
	public List<Tour> findAll() {
		List<Tour> allTours = new ArrayList<>();
		Iterable<Tour> iterable = tourRepository.findAll();
		iterable.forEach(allTours::add);
		return allTours;
	}

	@Override
	public Tour add(Tour tour) throws NotUniqueTourException {
		try {
			List<TourPhoto> photos = tour.getPhotos();
			tour = tourRepository.save(tour);
			long id = tour.getId();
			photos.forEach(photo -> photo.getTour().setId(id));
			tourPhotoRepository.saveAll(photos);
			return tourRepository.findById(id).get();
		} catch (DataIntegrityViolationException ex) {
			throw new NotUniqueTourException("The database contains a tour with this name");
		}
	}

	@Override
	public void update(Tour tour) throws NotFoundTourException {
		try {
			Tour oldTour = tourRepository.findById(tour.getId())
					.orElseThrow(() -> new NotFoundTourException("Tour was not found"));
			oldTour.getPhotos().forEach(tourPhotoRepository::delete);

			tour.getPhotos().forEach(photo -> photo.setTour(tour));
			add(tour);
		} catch (NotUniqueTourException e) {
			LOGGER.error("Can't update tour", e);
		}
	}

	public List<Tour> getByCriteria(FilterUnit filterUnit) {
		List<Long> ids = tourJdbcRepository.getIdToursByCriteria(filterUnit);
		List<Tour> tours = new ArrayList<>();
		ids.forEach(id -> tours.add(tourRepository.findById(id).get()));
		return tours;
	}

	@Override
	public List<Tour> getByUser(long userId) {
		Iterable<BigInteger> tourIds = tourRepository.findByUser(userId);
		List<Tour> result = new ArrayList<>();
		for (BigInteger id : tourIds) {
			result.add(tourRepository.findById(id.longValue()).orElseThrow(() -> new NotFoundException("Can't find tour")));
		}
		return result;
	}

	@Override
	public List<RegisteredUser> getByTour(long tourId) {
		Tour tour = tourRepository.findById(tourId).orElseThrow(() -> new NotFoundException("Can't find tour"));
		return tour.getParticipatingList().stream()
				.map(Participating::getUser)
				.collect(Collectors.toList());
	}

	@Override
	public Tour setGuide(Tour tour, Guide guide) {
		tour.setGuide(guide);
		return tourRepository.save(tour);
	}

	@Override
	public boolean checkGuideOnToursBetweenDates(long guideId, Date date, Date startDate, Date finishDate) {
		List<Tour> tours = tourRepository.findTours(guideId, date, startDate, finishDate);
		return !tours.isEmpty();
	}

}