package org.nure.gotrip.controller;

import org.nure.gotrip.controller.response.NotFoundException;
import org.nure.gotrip.dto.MessageAddingDto;
import org.nure.gotrip.exception.AppException;
import org.nure.gotrip.model.Chat;
import org.nure.gotrip.model.Message;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.ChatService;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.service.impl.MessageService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

import java.util.Date;

@Controller
@RequestMapping("/message")
public class MessageController {

    private MessageService messageService;
    private RegisteredUserService registeredUserService;
    private ChatService chatService;

    @Autowired
    public MessageController(MessageService messageService, RegisteredUserService registeredUserService, ChatService chatService) {
        this.messageService = messageService;
        this.registeredUserService = registeredUserService;
        this.chatService = chatService;
    }

    /*@PostMapping("/send")
    public ResponseEntity sendMessage(@RequestBody MessageAddingDto dto){
        RegisteredUser user;
        Chat chat;

        try{
            user = registeredUserService.findById(dto.getAuthorId());
            chat = chatService.findById(dto.getChatId());
        }catch (AppException e){
            throw new NotFoundException(e.getMessage());
        }

        Message message = new Message();
        message.setChat(chat);
        message.setAuthor(user);
        message.setText(dto.getText());
        message.setDateTime(new Date(System.currentTimeMillis()));

        message = messageService.createMessage(message);
        return new ResponseEntity<>(message, HttpStatus.OK);
    }*/
}
