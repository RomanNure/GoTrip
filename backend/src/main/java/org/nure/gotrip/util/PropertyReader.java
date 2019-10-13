package org.nure.gotrip.util;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.io.InputStream;
import java.io.UncheckedIOException;
import java.util.Properties;

@Component
public class PropertyReader {

    private static final String RESOURCE_NAME = "settings.properties";

    private Properties properties;

    @Autowired
    public PropertyReader(){
        properties = new Properties();
        InputStream stream = getClass().getClassLoader().getResourceAsStream(RESOURCE_NAME);
        try {
            properties.load(stream);
        } catch (IOException e) {
            throw new UncheckedIOException("Can't load properties from " + RESOURCE_NAME, e);
        }
    }

    public String getProperty(String key){
        return properties.getProperty(key);
    }
}
