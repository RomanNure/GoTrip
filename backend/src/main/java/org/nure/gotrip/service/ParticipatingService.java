package org.nure.gotrip.service;

import org.nure.gotrip.model.Participating;

import java.math.BigInteger;

public interface ParticipatingService {

    boolean isAbleToParticipate(long userId, long tourId);

    Participating participate(long userId, long tourId);
}
