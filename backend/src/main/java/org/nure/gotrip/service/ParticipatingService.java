package org.nure.gotrip.service;

import org.nure.gotrip.dto.PreparingDto;
import org.nure.gotrip.exception.NotFoundParticipatingException;
import org.nure.gotrip.model.Participating;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.model.Tour;

public interface ParticipatingService {

    boolean isAbleToParticipate(long userId, long tourId);

    Participating participate(PreparingDto dto);

    boolean prepare(PreparingDto dto);

    PreparingDto confirm(String orderId);

    String getStatus(String orderId);

    Participating getByTourAndUser(Tour tour, RegisteredUser user) throws NotFoundParticipatingException;
}
