package org.nure.gotrip.repository;

import org.nure.gotrip.model.Guide;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface GuideRepository extends CrudRepository<Guide, Long> {

	Optional<Guide> findGuideByRegisteredUserId(long id);

}