package org.nure.gotrip.util.validation;

import org.nure.gotrip.dto.CompanyDto;
import org.nure.gotrip.exception.ValidationException;
import org.springframework.stereotype.Component;

@Component
public class CompanyRegistrationValidator {

	private static final String EMAIL_PATTERN = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+" +
			"(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|" +
			"\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\" +
			"\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)" +
			"+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}" +
			"(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-" +
			"\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)])";

	private static final String NAME_PATTERN = "[a-zA-Z]{2,47}";

	public boolean registrationCompanyValid(CompanyDto companyDto) throws ValidationException {
		validateEmail(companyDto.getEmail());
		validateName(companyDto.getName());
		return true;
	}

	private void validateEmail(String email) throws ValidationException {
		if (isNotValidEmail(email)) {
			throw new ValidationException("Invalid email");
		}
	}

	private void validateName(String name) throws ValidationException {
		if (isNotValidName(name)) {
			throw new ValidationException("Invalid name");
		}
	}

	private boolean isNotValidName(String name) {
		return !name.matches(NAME_PATTERN);
	}

	private boolean isNotValidEmail(String email) {
		return !email.matches(EMAIL_PATTERN);
	}

}
