package org.nure.gotrip.model;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.*;
import java.util.Date;

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

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "administrator_id")
    private Administrator administrator;

    public Tour(){
        //Constructor for JPA
    }
}
