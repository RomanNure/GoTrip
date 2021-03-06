package org.nure.gotrip.service.impl;

import org.nure.gotrip.model.Tour;
import org.nure.gotrip.repository.TourRepository;
import org.nure.gotrip.service.TourService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class TourServiceImpl implements TourService {

    private TourRepository tourRepository;

    @Autowired
    public TourServiceImpl(TourRepository tourRepository) {
        this.tourRepository = tourRepository;
    }

    @Override
    public List<Tour> findAll(){
        List<Tour> allTours = new ArrayList<>();
        Iterable<Tour> iterable = tourRepository.findAll();
        iterable.forEach(allTours::add);
        return allTours;
    }
}
