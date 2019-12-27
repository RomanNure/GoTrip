package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.BadRequestException;
import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.ChatAddingDto;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.Chat;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.ChatService;
import org.nure.gotrip.service.RegisteredUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/chat")
public class ChatController {

    private ChatService chatService;
    private RegisteredUserService registeredUserService;

    @Autowired
    public ChatController(ChatService chatService, RegisteredUserService registeredUserService) {
        this.chatService = chatService;
        this.registeredUserService = registeredUserService;
    }

    @PostMapping("/get")
    public ResponseEntity getChat(@RequestBody ChatAddingDto dto){
        if(dto.getUserId1() == dto.getUserId2()){
            throw new BadRequestException("Wrong input");
        }

        RegisteredUser user1;
        RegisteredUser user2;

        try{
            user1 = registeredUserService.findById(dto.getUserId1());
            user2 = registeredUserService.findById(dto.getUserId2());
        } catch (NotFoundUserException e) {
            throw new NotFoundException(e.getMessage());
        }
        Chat chat = chatService.create(user1, user2);
        return new ResponseEntity<>(chat, HttpStatus.OK);
    }
}
