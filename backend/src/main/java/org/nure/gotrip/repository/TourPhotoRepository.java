package org.nure.gotrip.repository;

import org.nure.gotrip.model.TourPhoto;
import org.springframework.data.repository.CrudRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface TourPhotoRepository extends CrudRepository<TourPhoto, Long> {
}
