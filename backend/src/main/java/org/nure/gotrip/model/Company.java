package org.nure.gotrip.model;

import com.fasterxml.jackson.annotation.JsonIgnore;
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

	@JsonIgnore
	@OneToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "owner_id")
	private RegisteredUser owner;

	@Column(name = "name")
	private String name;

	@Column(name = "email")
	private String email;

    @OneToMany(mappedBy = "company", fetch = FetchType.LAZY)
	private List<Administrator> administrators;

	public Company() {
	    //Constructor for JPA
	}
}
