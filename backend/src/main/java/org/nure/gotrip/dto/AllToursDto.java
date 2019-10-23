package org.nure.gotrip.dto;

import org.nure.gotrip.model.Tour;

import java.util.List;

public class AllToursDto {

    private List<Tour> allTours;

    public AllToursDto(List<Tour> allTours) {
        this.allTours = allTours;
    }
}
