package org.nure.gotrip.model;

import com.fasterxml.jackson.annotation.JsonIgnore;

import javax.persistence.*;

@Entity
@Table(name = "tour_photos")
public class TourPhoto {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "tour_photo_id")
    private long id;

    @Column(name = "photo_url")
    private String url;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "tour_id")
    @JsonIgnore
    private Tour tour;

    public TourPhoto(){

    }

    public TourPhoto(String url, Tour tour){
        this.url = url;
        this.tour = tour;
    }
}
