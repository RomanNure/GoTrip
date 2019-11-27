package org.nure.gotrip.model.notification;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Getter
@Setter
@NoArgsConstructor
public class PlainTextNotificationData implements NotificationData {

    private static final String TYPE = "Plain";

    private String text;

    @Override
    public String data() throws JsonProcessingException {
        return new ObjectMapper().writeValueAsString(this);
    }

    @Override
    public String getType() {
        return TYPE;
    }
}
