package org.nure.gotrip.model;

import com.fasterxml.jackson.annotation.JsonIgnore;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

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

	public TourPhoto() {

	}

	public TourPhoto(String url, Tour tour) {
		this.url = url;
		this.tour = tour;
	}

	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
	}

	public Tour getTour() {
		return tour;
	}

	public void setTour(Tour tour) {
		this.tour = tour;
	}

	public long getId() {
		return id;
	}

	public void setId(long id) {
		this.id = id;
	}
}
