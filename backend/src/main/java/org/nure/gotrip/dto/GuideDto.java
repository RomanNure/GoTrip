package org.nure.gotrip.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.nure.gotrip.model.RegisteredUser;

@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class GuideDto {

	private long id;

	private RegisteredUser registeredUser;

	private String wantedToursKeyWords;

}