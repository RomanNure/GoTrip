package org.nure.gotrip.config;

import org.nure.gotrip.util.PropertyReader;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class AvatarConfiguration {

    private static final String DYNAMIC_IMAGES_ROOT = "images/";

    private PropertyReader propertyReader;

    @Autowired
    public AvatarConfiguration(PropertyReader propertyReader) {
        this.propertyReader = propertyReader;
    }

    @Bean
    @Qualifier("dynamicRoot")
    public String dynamicImagesRoot(){
        return DYNAMIC_IMAGES_ROOT;
    }

    @Bean
    @Qualifier("staticRoot")
    public String staticImagesRoot(){
        return propertyReader.getProperty("imagesRoot");
    }
}
