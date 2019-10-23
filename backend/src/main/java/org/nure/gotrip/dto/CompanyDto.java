package org.nure.gotrip.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;
import org.nure.gotrip.model.RegisteredUser;

import javax.validation.constraints.NotNull;

@Getter
@Setter
@AllArgsConstructor
public class CompanyDto {

    @NotNull
	private String name;

	@NotNull
	private String email;

	@NotNull
	private RegisteredUser owner;

	@NotNull
	private String description;
}
