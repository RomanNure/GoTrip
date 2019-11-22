package org.nure.gotrip.model.notification;

import com.fasterxml.jackson.core.JsonProcessingException;

public interface NotificationData {

    String getData() throws JsonProcessingException;

    String getType();
}
