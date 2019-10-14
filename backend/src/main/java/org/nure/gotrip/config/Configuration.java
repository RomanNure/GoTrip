package org.nure.gotrip.config;

import org.modelmapper.ModelMapper;
import org.nure.gotrip.util.PropertyReader;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;

@org.springframework.context.annotation.Configuration
public class Configuration {

    @Bean
	public ModelMapper modelMapper() {
		return new ModelMapper();
	}
}
