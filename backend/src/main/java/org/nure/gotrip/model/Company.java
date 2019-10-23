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
import javax.persistence.OneToMany;
import javax.persistence.OneToOne;
import javax.persistence.Table;
import java.util.List;

@Getter
@Setter
@Entity
@Table(name = "company")
public class Company {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "company_id")
	private long id;

	@JsonIgnore
	@OneToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "owner_id")
	private RegisteredUser owner;

	@Column(name = "name")
	private String name;

	@Column(name = "email")
	private String email;

	@Column(name = "description")
	private String description;

	@Column(name = "image_link")
	private String imageLink;

	@OneToMany(mappedBy = "company", fetch = FetchType.LAZY)
	private List<Administrator> administrators;

	public Company() {
		//Constructor for JPA
	}
}
