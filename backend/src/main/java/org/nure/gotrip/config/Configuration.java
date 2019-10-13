package org.nure.gotrip.config;

import org.modelmapper.ModelMapper;
import org.nure.gotrip.util.PropertyReader;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;

@org.springframework.context.annotation.Configuration
public class Configuration {

    private PropertyReader propertyReader;

    @Autowired
    public Configuration(PropertyReader propertyReader) {
        this.propertyReader = propertyReader;
    }

    @Bean
	public ModelMapper modelMapper() {
		return new ModelMapper();
	}

	@Bean
	public String imagesRoot(){
	    return propertyReader.getProperty("imagesRoot");
    }
}
