package org.nure.gotrip.service;

import org.nure.gotrip.exception.NotUniqueGuideException;
import org.nure.gotrip.model.Guide;

import java.util.Optional;

public interface GuideService {

	Guide add(Guide guide) throws NotUniqueGuideException;

	Optional<Guide> getById(long id);

	Optional<Guide> getByRegisteredUserId(long id);

}