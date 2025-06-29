package com.swd392.group7.phygen.Topic.Service;

import com.swd392.group7.phygen.Topic.Entity.Topic;
import com.swd392.group7.phygen.Topic.Exception.TopicException;
import com.swd392.group7.phygen.Topic.Repository.TopicRepo;
import lombok.RequiredArgsConstructor;
import org.modelmapper.ModelMapper;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class TopicService {
    private final TopicRepo topicRepo;
    private final ModelMapper modelMapper;

    public Topic findTopicById(Long id){
        return topicRepo.findById(id).orElseThrow(()-> new TopicException("Cant found topic", HttpStatus.NOT_FOUND));
    }
}
