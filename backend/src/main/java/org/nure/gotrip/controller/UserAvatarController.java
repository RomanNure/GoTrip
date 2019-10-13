package org.nure.gotrip.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;

@RestController
public class UserAvatarController {

    //A folder under 'target/static/', when uploaded images are located on a server
    private static final String DYNAMIC_IMAGES_ROOT = "images/";

    //A folder ith all existing images
    private String imagesRoot;

    @PostMapping(value = "/update/user/avatar")
    public void updateUserAvatar(@RequestParam("avatar") MultipartFile image){
        //TODO
    }

    @Autowired
    public void setImagesRoot(String imagesRoot) {
        this.imagesRoot = imagesRoot;
    }
}
