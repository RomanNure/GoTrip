package org.nure.gotrip.test;

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
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.stereotype.Controller;

import java.util.Date;

@Controller
public class ChatMessageController {

    private MessageService messageService;
    private RegisteredUserService registeredUserService;
    private ChatService chatService;

    @Autowired
    public ChatMessageController(MessageService messageService, RegisteredUserService registeredUserService, ChatService chatService) {
        this.messageService = messageService;
        this.registeredUserService = registeredUserService;
        this.chatService = chatService;
    }

    @MessageMapping("/hello")
    @SendTo("/topic/greetings")
    public Message greeting(MessageAddingDto dto) {
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
        return message;
    }
}
