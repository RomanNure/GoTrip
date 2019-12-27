package org.nure.gotrip.service.impl;

import org.nure.gotrip.exception.ChatNotFoundException;
import org.nure.gotrip.model.Chat;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.repository.ChatRepository;
import org.nure.gotrip.service.ChatService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Optional;

@Service
public class ChatServiceImpl implements ChatService {

    private ChatRepository chatRepository;

    @Autowired
    public ChatServiceImpl(ChatRepository chatRepository) {
        this.chatRepository = chatRepository;
    }

    @Override
    public Chat create(RegisteredUser user1, RegisteredUser user2) {
        Chat chat;
        Optional<Chat> opt = chatRepository.findByFirstUserAndSecondUser(user1, user2);
        if(!opt.isPresent()){
            opt = chatRepository.findByFirstUserAndSecondUser(user2, user1);
            if(!opt.isPresent()){
                chat = new Chat();
                chat.setFirstUser(user1);
                chat.setSecondUser(user2);
                return chatRepository.save(chat);
            }
            return opt.get();
        }
        return opt.get();
    }

    @Override
    public Chat findById(long id) throws ChatNotFoundException {
        return chatRepository.findById(id).orElseThrow(()->new ChatNotFoundException("Chat with such id was not found"));
    }
}
