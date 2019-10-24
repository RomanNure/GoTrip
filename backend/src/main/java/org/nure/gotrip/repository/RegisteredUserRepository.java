package org.nure.gotrip.repository;

import org.nure.gotrip.model.RegisteredUser;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.math.BigInteger;
import java.util.Optional;

@Repository
public interface RegisteredUserRepository extends CrudRepository<RegisteredUser, Long> {
    Optional<RegisteredUser> findByLogin(String login);

    @Query(value = "SELECT DISTINCT company.company_id FROM company, administrators, registered_user WHERE company.company_id = administrators.company_id AND administrators.registered_user_id = ?1", nativeQuery = true)
    Iterable<BigInteger> findByEmployee(Long userId);

    @Query(value="SELECT DISTINCT registered_user.registered_user_id FROM registered_user left join administrators on registered_user.registered_user_id = administrators.registered_user_id WHERE " +
            "administrators.administrator_id = ?1", nativeQuery = true)
    BigInteger findByAdministrator(long administratorId);
}