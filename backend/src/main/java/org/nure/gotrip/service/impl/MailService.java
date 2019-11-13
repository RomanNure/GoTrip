package org.nure.gotrip.service.impl;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.mail.Message;
import javax.mail.MessagingException;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.Properties;

@Service
public class MailService {

	private static final Logger logger = LoggerFactory.getLogger(MailService.class);

	private static final String KEY_WORD = "{{injection}}";

	private Properties mailProperties;

	public void sendThroughRemote(String recipient, String mailTemplate, String subject, String... placeholders) throws MessagingException {
		try {
			Session session = Session.getDefaultInstance(mailProperties);
			MimeMessage message = new MimeMessage(session);
			message.setFrom(new InternetAddress(mailProperties.getProperty("mail.smtp.user")));
			message.addRecipient(Message.RecipientType.TO, new InternetAddress(recipient));

			message.setSubject(subject);
			String content = formContent(mailTemplate, placeholders);
			message.setContent(content, "text/html; charset=utf-8");

			Transport tr = session.getTransport();
			tr.connect(mailProperties.getProperty("mail.smtp.user"), mailProperties.getProperty("password"));
			tr.sendMessage(message, message.getAllRecipients());
		} catch (MessagingException e) {
			logger.error(String.format("%s %s", "Mail was not sent to", recipient));
			throw e;
		}
	}

	private String formContent(String mailTemplate, String... placeholders) {
		StringBuilder mailBuilder = new StringBuilder(mailTemplate);
		int arrayIndex = 0;
		while (mailBuilder.indexOf(KEY_WORD) != -1) {
			if (placeholders.length < arrayIndex + 1) {
				throw new IllegalArgumentException("Not enough placeholders");
			}
			int i = mailBuilder.indexOf(KEY_WORD);
			String replaceString = placeholders[arrayIndex++];
			mailBuilder.replace(i, KEY_WORD.length() + i, replaceString);
		}
		return mailBuilder.toString();
	}

	public String getMailTemplate(String filename) throws IOException {
		FileReader fileReader = new FileReader(filename);
		BufferedReader reader = new BufferedReader(fileReader);
		StringBuilder builder = new StringBuilder();
		String line = reader.readLine();
		while (line != null) {
			builder.append(line);
			line = reader.readLine();
		}
		return builder.toString();
	}

	@Autowired
	private void setMailProperties(Properties mailProperties) {
		this.mailProperties = mailProperties;
	}

	public String getEmailProperty(String key) {
		return mailProperties.getProperty(key);
	}
}
