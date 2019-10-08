package org.nure.gotrip.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

import javax.validation.constraints.NotNull;

@Getter
@Setter
@AllArgsConstructor
public class UserRegistrationFormDto {

	@NotNull
	private String login;

	@NotNull
	private String password;

	@NotNull
	private String email;

}
