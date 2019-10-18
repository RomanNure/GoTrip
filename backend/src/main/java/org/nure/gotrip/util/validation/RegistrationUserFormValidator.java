package org.nure.gotrip.util.validation;

import org.nure.gotrip.exception.ValidationException;
import org.nure.gotrip.dto.UserRegistrationFormDto;
import org.nure.gotrip.model.RegisteredUser;
import org.springframework.stereotype.Component;

@Component
public class RegistrationUserFormValidator {

    private static final int MAX_FULLNAME_SIZE = 50;

	private static final String LOGIN_PATTERN = "[a-zA-Z0-9]{8,20}";

	private static final String PASS_PATTERN = "[a-zA-Z0-9]{8,30}";

	private static final String EMAIL_PATTERN = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+" +
			"(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|" +
			"\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\" +
			"\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)" +
			"+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}" +
			"(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-" +
			"\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)])";

	private static final String FULL_NAME_PATTERN = "[a-zA-Z]{2,47} [a-zA-Z]{2,47}";

    private static final String PHONE_PATTERN = "\\+[0-9]{7,15}";

    public boolean registrationUserFormValid(UserRegistrationFormDto userRegistrationFormDto) throws ValidationException {
		isRegistrationDataCorrect(
		        userRegistrationFormDto.getLogin(),
                userRegistrationFormDto.getEmail()
        );

        if (!userRegistrationFormDto.getPassword().matches(PASS_PATTERN)) {
            throw new ValidationException("Invalid password");
        }
		return true;
	}

	public void validateUser(RegisteredUser user) throws ValidationException{
        isRegistrationDataCorrect(
                user.getLogin(),
                user.getEmail()
        );

        isAdditionalDataCorrect(
                user.getFullName(),
                user.getPhone()
        );
    }

    private void isRegistrationDataCorrect(String login, String email) throws ValidationException{
        if (login == null || !login.matches(LOGIN_PATTERN)) {
            throw new ValidationException("Invalid login");
        }
        if (email == null || !email.matches(EMAIL_PATTERN)) {
            throw new ValidationException("Invalid email");
        }
    }

    private void isAdditionalDataCorrect(String fullname, String phone) throws ValidationException{
        if (fullname != null && !fullname.matches(FULL_NAME_PATTERN) && fullname.length() > MAX_FULLNAME_SIZE) {
            throw new ValidationException("Invalid full name");
        }
        if (phone != null && !phone.matches(PHONE_PATTERN)) {
            throw new ValidationException("Invalid phone");
        }
    }
}
