package org.nure.gotrip.dto;

import lombok.Getter;
import lombok.Setter;

import javax.validation.constraints.NotNull;

@Getter
@Setter
public class ParticipatingDto {

    @NotNull
    private long tourId;
}
