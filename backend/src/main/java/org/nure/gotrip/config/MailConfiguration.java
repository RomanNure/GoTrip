package org.nure.gotrip.config;

import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.io.*;
import java.util.Properties;

@Configuration
public class MailConfiguration {

    private static final String MAIL_PROPERTIES_FILENAME = "mail.properties";

    @Bean
    public Properties mailProperties() throws IOException {
        InputStream stream = getClass().getClassLoader().getResourceAsStream(MAIL_PROPERTIES_FILENAME);
        Properties properties = new Properties();
        properties.load(stream);
        return properties;
    }

    @Bean
    @Qualifier("mailTemplate")
    public String mailTemplate() throws IOException {
        FileReader fileReader = new FileReader("target/classes/mail.html");
        BufferedReader reader = new BufferedReader(fileReader);
        StringBuilder builder = new StringBuilder();
        String line = reader.readLine();
        while(line != null){
            builder.append(line);
            line = reader.readLine();
        }
        return replaceLink(builder);
    }

    private String replaceLink(StringBuilder builder) throws IOException {
        Properties properties = mailProperties();

        int addressPosition = builder.indexOf("mailaddress");
        builder.replace(addressPosition, addressPosition + "mailaddress".length(), properties.getProperty("mailaddress"));
        return builder.toString();
    }
}
