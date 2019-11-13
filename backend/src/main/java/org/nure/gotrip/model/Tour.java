package org.nure.gotrip.model;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import java.util.Date;
import java.util.List;

@Getter
@Setter
@Entity
@Table(name = "tours")
public class Tour {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "tour_id")
	private long id;

	@Column(name = "name")
	private String name;

	@Column(name = "description")
	private String description;

	@Column(name = "price_per_person")
	private double pricePerPerson;

	@Column(name = "main_picture_url")
	private String mainPictureUrl;

	@Column(name = "start_date_time")
	private Date startDateTime;

	@Column(name = "finish_date_time")
	private Date finishDateTime;

	@Column(name = "max_participants")
	private int maxParticipants;

	@Column(name = "location")
	private String location;

	@ManyToOne(fetch = FetchType.EAGER)
	@JoinColumn(name = "administrator_id")
	private Administrator administrator;

	@OneToMany(mappedBy = "tour", fetch = FetchType.LAZY)
	private List<TourPhoto> photos;

	@OneToMany(mappedBy = "tour", fetch = FetchType.LAZY)
	private List<Participating> participatingList;

	public Tour() {
		//Constructor for JPA
	}
}
