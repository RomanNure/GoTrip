package org.nure.gotrip.service;

import org.nure.gotrip.model.Participating;

public interface ParticipatingService {

    boolean isAbleToParticipate(long userId, long tourId);

    Participating participate(long userId, long tourId);
}
