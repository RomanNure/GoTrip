package org.nure.gotrip.model.notification;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
public class OfferGuidingNotificationData implements NotificationData{

    static final String TYPE = "OfferGuiding";

    long tourId;
    long guideId;
    long companyId;
    double sum;

    @Override
    public String data() throws JsonProcessingException {
        return new ObjectMapper().writeValueAsString(this);
    }

    @Override
    public String getType() {
        return TYPE;
    }
}
