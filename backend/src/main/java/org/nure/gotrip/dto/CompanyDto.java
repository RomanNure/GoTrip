package org.nure.gotrip.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
@AllArgsConstructor
public class CompanyDto {

	private String name;
	private String email;
	private long idOwner;

	public CompanyDto() {
	}

}
