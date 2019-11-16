package org.nure.gotrip.util.session;

import java.util.HashMap;
import java.util.Map;

public class AppSession {

    private Map<String, Object> attributes = new HashMap<>();

    public void setAttribute(String key, Object value){
        this.attributes.put(key, value);
    }

    public Object getAttribute(String key){
        return this.attributes.get(key);
    }
}
