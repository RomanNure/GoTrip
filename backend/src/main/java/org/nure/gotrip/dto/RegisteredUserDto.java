package org.nure.gotrip.dto;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.model.RegisteredUser;

import java.math.BigInteger;

@Getter
@Setter
@NoArgsConstructor
public class RegisteredUserDto {

	private RegisteredUser registeredUser;
	private Iterable<BigInteger> administrators;
	private Iterable<BigInteger> companies;
	private Guide guide;

}