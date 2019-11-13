package org.nure.gotrip.exception;

import java.sql.SQLException;

public class RuntimeDatabaseException extends RuntimeException {

	public RuntimeDatabaseException(SQLException e) {
		super(e);
	}
}
