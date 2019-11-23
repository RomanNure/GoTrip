package org.nure.gotrip.model.notification;

import com.fasterxml.jackson.core.JsonProcessingException;

public interface NotificationData {

    String data() throws JsonProcessingException;

    String getType();
}
