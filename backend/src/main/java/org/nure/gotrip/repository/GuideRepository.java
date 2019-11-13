package org.nure.gotrip.repository;

import org.nure.gotrip.model.Guide;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface GuideRepository extends CrudRepository<Guide, Long> {
}
