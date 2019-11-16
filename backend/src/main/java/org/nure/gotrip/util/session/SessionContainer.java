package org.nure.gotrip.util.session;

import org.springframework.stereotype.Component;

import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

@Component
public class SessionContainer {

    private Map<String, AppSession> sessions = new HashMap<>();

    public String createSession(){
        String sessionId = UUID.randomUUID().toString();
        sessions.put(sessionId, new AppSession());
        return sessionId;
    }

    public AppSession getSession(String sessionId){
        return sessions.get(sessionId);
    }
}
