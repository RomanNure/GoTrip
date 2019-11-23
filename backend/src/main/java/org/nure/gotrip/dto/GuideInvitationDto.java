package org.nure.gotrip.dto;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class GuideInvitationDto {

    private long tourId;
    private long guideId;
    private double sum;
}
