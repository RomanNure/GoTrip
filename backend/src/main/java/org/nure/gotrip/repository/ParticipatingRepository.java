package org.nure.gotrip.repository;

import org.nure.gotrip.model.Participating;
import org.nure.gotrip.model.Tour;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.math.BigInteger;
import java.util.Date;
import java.util.List;

@Repository
public interface ParticipatingRepository extends CrudRepository<Participating, Long> {

	@Query(value = "SELECT DISTINCT registered_user.registered_user_id FROM registered_user left join participating on participating.registered_user_id = registered_user.registered_user_id WHERE " +
			"participating.participating_id = ?1", nativeQuery = true)
	BigInteger findUser(long participatingId);

	@Query(value = "SELECT t.* from participating " +
			"INNER JOIN tours t ON participating.tour_id = t.tour_id " +
			"INNER JOIN guide g ON t.guide_id = g.guide_id " +
			"WHERE participating.registered_user_id = ?1 AND " +
			"(?2 BETWEEN t.start_date_time and t.finish_date_time OR " +
			"(?3 >= t.start_date_time AND ?4 <= t.finish_date_time) OR " +
			"(?3 >= t.start_date_time AND ?3 <= t.finish_date_time) OR " +
			"(?4 >= t.start_date_time AND ?4 <= t.finish_date_time))", nativeQuery = true)
	List<Tour> findTours(long userId, Date nowDate, Date startDate, Date finishDate);

}