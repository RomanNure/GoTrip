package org.nure.gotrip.repository;

import org.nure.gotrip.model.Company;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.math.BigInteger;
import java.util.Optional;

@Repository
public interface CompanyRepository extends JpaRepository<Company, Long> {

	Optional<Company> findByName(String name);

    @Query(value="SELECT DISTINCT company.company_id FROM company left join administrators on company.company_id = administrators.company_id WHERE " +
            "administrators.administrator_id = ?1", nativeQuery = true)
    BigInteger findByAdministrator(long administratorId);
}