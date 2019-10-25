package org.nure.gotrip.dto;

import lombok.Getter;
import lombok.Setter;

import java.util.Date;

@Getter
@Setter
public class TourDto {

	private String name;
	private String description;
	private double pricePerPerson;
	private String mainPictureUrl;
	private Date startDateTime;
	private Date finishDateTime;
	private int maxParticipants;
	private long idAdministrator;

}