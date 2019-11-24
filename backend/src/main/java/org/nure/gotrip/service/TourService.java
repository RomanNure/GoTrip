package org.nure.gotrip.service;

import org.nure.gotrip.dto.FilterUnit;
import org.nure.gotrip.exception.NotFoundTourException;
import org.nure.gotrip.exception.NotUniqueTourException;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.Tour;

import java.util.Date;
import java.util.List;

public interface TourService {

	List<Tour> findAll();

	Tour add(Tour tour) throws NotUniqueTourException;

	void update(Tour tour) throws NotFoundTourException;

	Tour findById(long id) throws NotFoundTourException;

	List<Tour> getByCriteria(FilterUnit filterUnit);

	List<Tour> getByUser(long userId);

	List<RegisteredUser> getByTour(long tourId);

	Tour setGuide(Tour tour, Guide guide);

	boolean checkGuideOnToursBetweenDates(long guideId, Date date, Date startDate, Date finishDate);
}
