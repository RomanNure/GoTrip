package org.nure.gotrip.util.validation;

import org.nure.gotrip.dto.AddGuideDto;
import org.nure.gotrip.exception.ValidationException;
import org.springframework.stereotype.Component;

@Component
public class GuideAddValidator {

	public void guideValid(AddGuideDto addGuideDto) throws ValidationException {
		validateUserId(addGuideDto.getIdRegisteredUser());
	}

	private void validateUserId(long id) throws ValidationException {
		if (isNotValidUserId(id)) {
			throw new ValidationException("Invalid user id! Id must be more then zero.");
		}
	}

	private boolean isNotValidUserId(long id) {
		return id <= 0;
	}

}