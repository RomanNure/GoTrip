package org.nure.gotrip.util;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;

import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Base64;
import java.util.Formatter;

@Component
public class Encoder {

	private static final byte DEFAULT_BYTE = 5;
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
		for (int i = 0; i < encodedBytes.length; ++i) {
			if (encodedBytes[i] == 0) {
				encodedBytes[i] = DEFAULT_BYTE;
			}
		}
		return new String(encodedBytes);
	}

    public String encodeSHA1(String line){
        String sha1;
        try
        {
            MessageDigest crypt = MessageDigest.getInstance("SHA-1");
            crypt.reset();
            crypt.update(line.getBytes(StandardCharsets.UTF_8));
            sha1 = byteToHex(crypt.digest());
        }
        catch(NoSuchAlgorithmException e)
        {
            logger.error("Algorithm SHA-1 was not found");
            throw new UnsupportedOperationException(e);
        }
        return sha1;
    }

    public String encodeBase64(String line){
        return Base64.getEncoder().encodeToString(line.getBytes());
    }

    public String decodeBase64(String line){
	    return new String(java.util.Base64.getDecoder().decode(line));
    }

    private String byteToHex(final byte[] hash)
    {
        Formatter formatter = new Formatter();
        for (byte b : hash)
        {
            formatter.format("%02x", b);
        }
        String result = formatter.toString();
        formatter.close();
        return result;
    }
}
