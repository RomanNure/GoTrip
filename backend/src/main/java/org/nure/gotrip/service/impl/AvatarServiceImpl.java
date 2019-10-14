package org.nure.gotrip.service.impl;

import org.nure.gotrip.service.AvatarService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.stereotype.Service;

import java.io.InputStream;

@Service
public class AvatarServiceImpl implements AvatarService {

    //A folder under 'target/static/', when uploaded images are located on a server
    private String dynamicImagesRoot;

    //A folder with all existing images
    private String staticImagesRoot;

    @Override
    public void saveAvatar(String filename, InputStream imageStream) {
        System.out.println("Saving image in " + "filename");
    }

    @Autowired
    @Qualifier("dynamicRoot")
    private void setDynamicImagesRoot(String dynamicRoot){
        dynamicImagesRoot = dynamicRoot;
    }

    @Autowired
    @Qualifier("staticRoot")
    private void setImagesRoot(String staticRoot) {
        staticImagesRoot = staticRoot;
    }


}
