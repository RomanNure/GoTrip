package org.nure.gotrip.util.validation;

import org.nure.gotrip.exception.ValidationException;
import org.nure.gotrip.dto.UserRegistrationFormDto;
import org.springframework.stereotype.Component;

@Component
public class RegistrationUserFormValidator {

	private static final String LOGIN_PATTERN = "[a-zA-Z0-9]{8,20}";

	private static final String PASS_PATTERN = "[a-zA-Z0-9]{8,30}";

	private static final String EMAIL_PATTERN = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+" +
			"(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|" +
			"\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\" +
			"\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)" +
			"+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}" +
			"(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-" +
			"\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)])";

	public boolean registrationUserFormValid(UserRegistrationFormDto userRegistrationFormDto) throws ValidationException {
		if (!userRegistrationFormDto.getLogin().matches(LOGIN_PATTERN)) {
			throw new ValidationException("Invalid login");
		}
		if (!userRegistrationFormDto.getPassword().matches(PASS_PATTERN)) {
			throw new ValidationException("Invalid password");
		}
		if (!userRegistrationFormDto.getEmail().matches(EMAIL_PATTERN)) {
			throw new ValidationException("Invalid email");
		}
		return true;
	}

}
