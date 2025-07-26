package com.swd392.group7.phygen.Topic.Service;

import com.swd392.group7.phygen.Subject.Entity.Subject;
import com.swd392.group7.phygen.Subject.Service.SubjectService;
import com.swd392.group7.phygen.Topic.Dto.TopicGeminiPromptResponse;
import com.swd392.group7.phygen.Topic.Entity.Topic;
import com.swd392.group7.phygen.Topic.Exception.TopicException;
import com.swd392.group7.phygen.Topic.Repository.TopicRepo;
import lombok.RequiredArgsConstructor;
import org.modelmapper.ModelMapper;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class TopicService {
    private final TopicRepo topicRepo;
    private final ModelMapper modelMapper;
    private final SubjectService subjectService;

    public Topic findTopicById(Long id){
        return topicRepo.findById(id).orElseThrow(()-> new TopicException("Cant found topic", HttpStatus.NOT_FOUND));
    }

    public List<TopicGeminiPromptResponse> topicStructureGeminiPrompt(Long subjectId){

        List<Topic> allTopicsInSubject = topicRepo.findAllBySubjectId(subjectId)
                .orElseThrow(() -> new TopicException("No topics found for subject ID: " + subjectId, HttpStatus.NOT_FOUND));
        List<Topic> rootTopics = allTopicsInSubject.stream()
                .filter(topic -> topic.getParent() == null)
                .toList();

        List<TopicGeminiPromptResponse> rootTopicResponses = new ArrayList<>();

        for (Topic rootTopic : rootTopics) {
            // Xây dựng cây TopicGeminiPromptResponse cho từng chủ đề gốc
            TopicGeminiPromptResponse rootTopicResponse = buildSimpleTopicTree(rootTopic);
            rootTopicResponses.add(rootTopicResponse);
        }
        return rootTopicResponses;

    }
    private TopicGeminiPromptResponse buildSimpleTopicTree(Topic currentTopic) {
        TopicGeminiPromptResponse topicResponse = new TopicGeminiPromptResponse();
        topicResponse.setName(currentTopic.getName());
        topicResponse.setLevel(currentTopic.getLevel()); // Giả sử Topic entity có trường Level

        topicResponse.setSubTopics(new ArrayList<>());

        // Tìm và thêm các chủ đề con
        List<Topic> subTopics = topicRepo.findAllByParentId(currentTopic.getId()).orElseThrow(()-> new TopicException("No subtopic found",HttpStatus.NOT_FOUND));

        for (Topic subTopic : subTopics) {
            TopicGeminiPromptResponse subTopicResponse = buildSimpleTopicTree(subTopic);
            topicResponse.getSubTopics().add(subTopicResponse);
        }

        return topicResponse;
    }
}
