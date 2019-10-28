package org.nure.gotrip.service.impl;

import org.nure.gotrip.exception.NotFoundTourException;
import org.nure.gotrip.exception.NotUniqueTourException;
import org.nure.gotrip.model.Tour;
import org.nure.gotrip.model.TourPhoto;
import org.nure.gotrip.repository.TourPhotoRepository;
import org.nure.gotrip.repository.TourRepository;
import org.nure.gotrip.service.TourService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class TourServiceImpl implements TourService {

	private TourRepository tourRepository;
    private TourPhotoRepository tourPhotoRepository;

	@Autowired
	public TourServiceImpl(TourRepository tourRepository, TourPhotoRepository tourPhotoRepository) {
		this.tourRepository = tourRepository;
        this.tourPhotoRepository = tourPhotoRepository;
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
}
