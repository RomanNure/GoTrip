package org.nure.gotrip.repository;

import org.nure.gotrip.model.Company;
import org.springframework.data.repository.CrudRepository;

import java.util.Optional;

public interface CompanyRepository extends CrudRepository<Company, Long> {

	Optional<Company> findByName(String name);

}