package org.nure.gotrip.util;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;

import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

@Component
public class Encoder {

	private final Logger logger = LoggerFactory.getLogger(Encoder.class);

	private static final String ENCODING = "MD5";

	public String encode(String line) {
		byte[] bytesOfMessage = line.getBytes(StandardCharsets.UTF_8);

		MessageDigest md;
		try {
			md = MessageDigest.getInstance(ENCODING);
		} catch (NoSuchAlgorithmException e) {
			logger.error("Algorithm MD5 was not found");
			throw new UnsupportedOperationException(e);
		}

		byte[] encodedBytes = md.digest(bytesOfMessage);
		return new String(encodedBytes);
	}
}
