package org.nure.gotrip.model;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.EqualsAndHashCode;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import javax.persistence.*;
import java.util.List;

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

	@OneToOne(fetch = FetchType.EAGER)
	@JoinColumn(name = "registered_user_id")
	private RegisteredUser registeredUser;

	@Column(name = "wanted_tours_keywords")
	private String wantedToursKeyWords;

	@Column(name = "card_number")
	private String cardNumber;

	@OneToMany(mappedBy = "guide", fetch = FetchType.LAZY)
    @JsonIgnoreProperties({"hibernateLazyInitializer", "guide"})
	private List<Tour> tours;

	public Guide(){

    }
}