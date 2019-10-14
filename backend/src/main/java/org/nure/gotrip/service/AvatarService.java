package org.nure.gotrip.service;

import java.io.InputStream;

public interface AvatarService {
    void saveAvatar(String filename, InputStream imageStream);
}
