package org.nure.gotrip.dto;

import org.junit.Assert;
import org.junit.Test;
import org.modelmapper.ModelMapper;
import org.nure.gotrip.model.RegisteredUser;

public class UserRegistrationFormDtoTest {

	private static final ModelMapper modelMapper = new ModelMapper();

	@Test
	public void checkUserRegistrationFormMapping() {
		UserRegistrationFormDto userRegistrationFormDto = new UserRegistrationFormDto("LolTest", "123456", "sobaka@mail.com", "Some text");

		RegisteredUser registeredUser = modelMapper.map(userRegistrationFormDto, RegisteredUser.class);

		Assert.assertEquals(userRegistrationFormDto.getLogin(), registeredUser.getLogin());
		Assert.assertEquals(userRegistrationFormDto.getPassword(), registeredUser.getPassword());
		Assert.assertEquals(userRegistrationFormDto.getEmail(), registeredUser.getEmail());
	}

}