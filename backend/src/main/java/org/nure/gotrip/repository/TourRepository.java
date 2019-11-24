package org.nure.gotrip.repository;

import org.nure.gotrip.model.Tour;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.math.BigInteger;
import java.util.Date;
import java.util.List;

@Repository
public interface TourRepository extends CrudRepository<Tour, Long> {
	@Query(value = "SELECT DISTINCT tours.tour_id FROM (participating left join tours ON participating.tour_id = tours.tour_id)" +
			" WHERE participating.registered_user_id = ?1", nativeQuery = true)
	Iterable<BigInteger> findByUser(Long userId);

	@Query(value = "select tours.* from tours " +
			"inner join guide on tours.guide_id = guide.guide_id " +
			"where guide.guide_id = ?1 AND " +
			"(?2 BETWEEN tours.start_date_time and tours.finish_date_time OR " +
			"(?3 >= tours.start_date_time AND ?4 <= tours.finish_date_time) OR " +
			"(?3 >= tours.start_date_time AND ?3 <= tours.finish_date_time) OR " +
			"(?4 >= tours.start_date_time AND ?4 <= tours.finish_date_time))", nativeQuery = true)
	List<Tour> findTours(long guideId, Date nowDate, Date startDate, Date finishDate);



}