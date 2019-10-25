package org.nure.gotrip.service;

import org.nure.gotrip.exception.NotUniqueTourException;
import org.nure.gotrip.model.Tour;

import java.util.List;

public interface TourService {

    List<Tour> findAll();

    Tour add(Tour tour) throws NotUniqueTourException;
}
