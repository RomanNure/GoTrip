package org.nure.gotrip.repository;

import org.nure.gotrip.dto.PreparingDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.util.HashMap;
import java.util.Map;

@Component
public class ParticipatingPreparationRepository {

    private Map<String, PreparingDto> map;

    @Autowired
    private ParticipatingPreparationRepository(){
        map = new HashMap<>();
    }

    public void add(PreparingDto dto){
        map.put(dto.getOrderId(), dto);
    }

    public PreparingDto remove(String orderId){
        return map.remove(orderId);
    }
}
