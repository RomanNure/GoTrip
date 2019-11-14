package org.nure.gotrip.repository;

import org.nure.gotrip.model.Tour;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.math.BigInteger;

@Repository
public interface TourRepository extends CrudRepository<Tour, Long> {
    @Query(value = "SELECT DISTINCT tours.tour_id FROM (participating left join tours ON participating.tour_id = tours.tour_id)" +
            " WHERE participating.registered_user_id = ?1", nativeQuery = true)
    Iterable<BigInteger> findByUser(Long userId);
}