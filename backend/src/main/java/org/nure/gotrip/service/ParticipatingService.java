package org.nure.gotrip.service;

public interface ParticipatingService {

    boolean isAbleToParticipate(long userId, long tourId);

    void participate(long userId, long tourId);
}
