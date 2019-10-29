package org.nure.gotrip.model;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.*;
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

	@ManyToOne(fetch = FetchType.EAGER)
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

	@Column(name = "domain")
	private String domain;

	@Column(name = "phone")
	private String phone;

	@Column(name = "address")
	private String address;

	@OneToMany(mappedBy = "company", fetch = FetchType.LAZY)
	private List<Administrator> administrators;

	public Company() {
		//Constructor for JPA
	}


}