package org.nure.gotrip.service;

import org.nure.gotrip.model.Guide;

import java.util.Optional;

public interface GuideService {

	Guide add(Guide guide);

	Optional<Guide> getById(long id);

}