package org.nure.gotrip.repository;

import org.nure.gotrip.model.Participating;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.math.BigInteger;

@Repository
public interface ParticipatingRepository extends CrudRepository<Participating, Long> {

    @Query(value = "SELECT DISTINCT registered_user.registered_user_id FROM registered_user left join participating on participating.registered_user_id = registered_user.registered_user_id WHERE " +
            "participating.participating_id = ?1", nativeQuery = true)
    BigInteger findUser(long participatingId);

}
