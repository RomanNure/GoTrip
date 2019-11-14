package org.nure.gotrip.repository;

import org.nure.gotrip.model.Participating;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.math.BigInteger;

@Repository
public interface ParticipatingRepository extends CrudRepository<Participating, Long> {



}
