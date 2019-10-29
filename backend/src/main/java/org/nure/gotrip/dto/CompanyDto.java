package org.nure.gotrip.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.nure.gotrip.model.RegisteredUser;

import javax.validation.constraints.NotNull;

@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
public class CompanyDto {

	@NotNull
	private String name;

	@NotNull
	private String email;

	@NotNull
	private RegisteredUser owner;

	@NotNull
	private String description;

	@NotNull
	private String phone;

	@NotNull
	private String imageLink;

	@NotNull
	private String address;

	@NotNull
	private String domain;

	public CompanyDto(String name, String email, RegisteredUser registeredUser, String description) {
		this.name = name;
		this.email = email;
		this.owner = registeredUser;
		this.description = description;
	}

}