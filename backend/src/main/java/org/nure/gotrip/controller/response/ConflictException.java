package org.nure.gotrip.controller.response;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

/**
 * class that is used to send error with HTTP-code 409
 */
@ResponseStatus(HttpStatus.CONFLICT)
public class ConflictException extends RuntimeException {
	public ConflictException(String message) {
		super(message);
	}
}
