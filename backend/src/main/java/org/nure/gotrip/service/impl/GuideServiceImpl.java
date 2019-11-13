package org.nure.gotrip.service.impl;

import org.nure.gotrip.model.Guide;
import org.nure.gotrip.repository.GuideRepository;
import org.nure.gotrip.service.GuideService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class GuideServiceImpl implements GuideService {

	private GuideRepository guideRepository;

	@Autowired
	public GuideServiceImpl(GuideRepository guideRepository) {
		this.guideRepository = guideRepository;
	}

	@Override
	public Guide add(Guide guide) {
		return guideRepository.save(guide);
	}

	@Override
	public Optional<Guide> getById(long id) {
		return guideRepository.findById(id);
	}

}