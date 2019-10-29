package org.nure.gotrip.model;


import com.fasterxml.jackson.annotation.JsonIgnore;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.*;

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

    @Column(name="tour_rate")
    private int tourRate;

    @Column(name = "ticket_hash")
    private String hash;

    @Column(name = "guide_rate")
    private int guideRate;

    public Participating(){
        //Constructor for JPA
    }
}
