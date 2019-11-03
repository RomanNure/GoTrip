package org.nure.gotrip.dto;

import com.fasterxml.jackson.annotation.JsonFormat;
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
    //@JsonFormat(pattern="yyyy-MM-dd HH:mm:ss")
	private Date startDateTime;
    //@JsonFormat(pattern="yyyy-MM-dd HH:mm:ss")
	private Date finishDateTime;
	private int maxParticipants;
	private long idAdministrator;
    private String[] photosUrl;
    private String location;
}