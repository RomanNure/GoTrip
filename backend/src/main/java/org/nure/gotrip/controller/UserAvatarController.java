package org.nure.gotrip.controller;

import org.nure.gotrip.service.AvatarService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;

@RestController
public class UserAvatarController {


    private AvatarService avatarService;

    @Autowired
    public UserAvatarController(AvatarService avatarService) {
        this.avatarService = avatarService;
    }

    @PostMapping(value = "/update/user/avatar")
    public void updateUserAvatar(@RequestParam long id, @RequestParam("avatar") MultipartFile image){
        String filename = getFilename(id, image);
        try {
            avatarService.saveAvatar(filename, image.getInputStream());
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private String getFilename(long id, @RequestParam("avatar") MultipartFile image){
        String imageName = image.getOriginalFilename();
        if(imageName == null){
            throw new IllegalArgumentException("No image");
        }
        int place = imageName.lastIndexOf('.');
        String extension = imageName.substring(place).toLowerCase();
        return String.format("%d%s", id, extension);
    }
}
