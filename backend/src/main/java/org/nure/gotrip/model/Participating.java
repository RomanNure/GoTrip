package org.nure.gotrip.model;

import com.fasterxml.jackson.annotation.JsonIgnore;
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
import javax.persistence.Table;

@Getter
@Setter
@Entity
@Table(name = "participating")
public class Participating {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "participating_id")
	private long id;

	@JsonIgnore
	@ManyToOne(fetch = FetchType.EAGER)
	@JoinColumn(name = "registered_user_id")
	private RegisteredUser user;

	@JsonIgnore
	@ManyToOne(fetch = FetchType.EAGER)
	@JoinColumn(name = "tour_id")
	private Tour tour;

	@Column(name = "tour_rate")
	private int tourRate;

	@Column(name = "guide_rate")
	private int guideRate;

	@Column(name="order_id")
	private String orderId;

	@Column(name="participated")
	private boolean participated;

	public Participating() {
		//Constructor for JPA
	}
}
