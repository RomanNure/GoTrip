package org.nure.gotrip.util.validation;

import org.nure.gotrip.dto.AddGuideDto;
import org.nure.gotrip.exception.ValidationException;
import org.springframework.stereotype.Component;

@Component
public class GuideAddValidator {

	private static final String PATTERN_CARD_NUMBER = "\\d{16}";

	public void guideValid(AddGuideDto addGuideDto) throws ValidationException {
		validateUserId(addGuideDto.getIdRegisteredUser());
		validateCardNumber(addGuideDto.getCardNumber());
	}

	private void validateUserId(long id) throws ValidationException {
		if (isNotValidUserId(id)) {
			throw new ValidationException("Invalid user id! Id must be more then zero.");
		}
	}

	private void validateCardNumber(String cardNumber) throws ValidationException {
		if (isNotValidCardNumber(cardNumber)) {
			throw new ValidationException("Invalid user card number! Card number must has 16 numbers.");
		}
	}

	private boolean isNotValidCardNumber(String cardNumber) {
		return !cardNumber.matches(PATTERN_CARD_NUMBER);
	}

	private boolean isNotValidUserId(long id) {
		return id <= 0;
	}

}