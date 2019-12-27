package org.nure.gotrip.service;

import org.nure.gotrip.exception.ChatNotFoundException;
import org.nure.gotrip.model.Chat;
import org.nure.gotrip.model.RegisteredUser;

public interface ChatService {

    Chat create(RegisteredUser user1, RegisteredUser user2);

    Chat findById(long id) throws ChatNotFoundException;
}
