package org.nure.gotrip.util.validation;

import org.nure.gotrip.dto.CompanyDto;
import org.nure.gotrip.exception.ValidationException;
import org.springframework.stereotype.Component;

@Component
public class CompanyRegistrationValidator {

	private static final String EMAIL_PATTERN = "^(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+" +
			"(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|" +
			"\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\" +
			"\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)" +
			"+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}" +
			"(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-" +
			"\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)])$";

	private static final String NAME_PATTERN = "^[a-zA-Z]{2,47}$";

	private static final String PHONE_NUMBER_PATTERN = "^(\\+*[0-9]{2}[.\\-\\s]?|00[.\\-\\s]?[0-9]{2}|0)([0-9]{1,3}[.\\-\\s]?(?:[0-9]{2}[.\\-\\s]?){4})$";

	private static final String DOMAIN_PATTERN = "(^|\\s)((https?:\\/\\/)?[\\w-]+(\\.[a-z-]+)+\\.?(:\\d+)?(\\/\\S*)?)";

	private static final String ADDRESS_PATTERN = "(.+)";

	private static final String IMAGE_LINK_PATTERN = "(?!.*?\\/)(.*?)(?:\\.(?:jpg|jpeg|jpe|png|img))";

	public void registrationCompanyValid(CompanyDto companyDto) throws ValidationException {
		validateEmail(companyDto.getEmail());
		validateName(companyDto.getName());
		validatePhone(companyDto.getPhone());
		validateDomain(companyDto.getDomain());
		validateAddress(companyDto.getAddress());
		validateImageLink(companyDto.getImageLink());
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

	private void validatePhone(String phone) throws ValidationException {
		if (isNotValidPhone(phone)) {
			throw new ValidationException("Invalid phone");
		}
	}

	private void validateDomain(String domain) throws ValidationException {
		if (isNotValidDomain(domain)) {
			throw new ValidationException("Invalid domain");
		}
	}

	private void validateAddress(String address) throws ValidationException {
		if (isNotValidAddress(address)) {
			throw new ValidationException("Invalid address");
		}
	}

	private void validateImageLink(String imageLink) throws ValidationException {
		if (isNotValidImageLink(imageLink)) {
			throw new ValidationException("Invalid imageLink");
		}
	}

	private boolean isNotValidName(String name) {
		return !name.matches(NAME_PATTERN);
	}

	private boolean isNotValidEmail(String email) {
		return !email.matches(EMAIL_PATTERN);
	}

	private boolean isNotValidPhone(String phone) {
		return !phone.matches(PHONE_NUMBER_PATTERN);
	}

	private boolean isNotValidDomain(String domain) {
		return !domain.matches(DOMAIN_PATTERN);
	}

	private boolean isNotValidAddress(String address) {
		return !address.matches(ADDRESS_PATTERN);
	}

	private boolean isNotValidImageLink(String imageLink) {
		return !imageLink.matches(IMAGE_LINK_PATTERN);
	}

}
