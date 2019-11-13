package org.nure.gotrip.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

@Configuration
public class MailConfiguration {

	private static final String MAIL_PROPERTIES_FILEPATH = "src/main/resources/mail.properties";

	@Bean
	public Properties mailProperties() throws IOException {
		InputStream stream = new FileInputStream(MAIL_PROPERTIES_FILEPATH);
		Properties properties = new Properties();
		properties.load(stream);
		return properties;
	}
}
