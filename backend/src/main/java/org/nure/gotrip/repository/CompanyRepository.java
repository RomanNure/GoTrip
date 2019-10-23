package org.nure.gotrip.repository;

import org.nure.gotrip.model.Company;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface CompanyRepository extends CrudRepository<Company, Long> {

	Optional<Company> findByName(String name);
}