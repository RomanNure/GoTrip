package org.nure.gotrip.repository;

import org.nure.gotrip.model.Tour;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface TourRepository extends CrudRepository<Tour, Long> {

}