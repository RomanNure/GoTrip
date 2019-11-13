package org.nure.gotrip.model;

import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.OneToOne;
import javax.persistence.Table;

@Getter
@Setter
@EqualsAndHashCode
@ToString
@Entity
@Table(name = "guide")
public class Guide {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "guide_id")
	private long id;

	@OneToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "registered_user_id")
	private RegisteredUser registeredUser;

	@Column(name = "wanted_tours_keywords")
	private String wantedToursKeyWords;

	public Guide() {
	}

}