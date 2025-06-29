package com.swd392.group7.phygen.Question.Service;

import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Question.Dto.Response;
import com.swd392.group7.phygen.Question.Dto.CreateRequest;
import com.swd392.group7.phygen.Question.Dto.UpdateRequest;
import com.swd392.group7.phygen.Question.Entity.Question;
import com.swd392.group7.phygen.Question.Enum.Status;
import com.swd392.group7.phygen.Question.Exception.QuestionException;
import com.swd392.group7.phygen.Question.Repository.QuestionRepo;
import com.swd392.group7.phygen.Topic.Entity.Topic;
import com.swd392.group7.phygen.Topic.Service.TopicService;
import lombok.RequiredArgsConstructor;
import org.modelmapper.ModelMapper;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class QuestionService {
    private final TopicService topicService;
    private final QuestionRepo questionRepo;
    private final ModelMapper modelMapper;

    public Question findQuestionById(Long id){
        return questionRepo.findById(id).orElseThrow(()-> new QuestionException("Cant found Question", HttpStatus.NOT_FOUND));
    }

    public Response create(CreateRequest questionRq,Long topicId){
        Topic topic = topicService.findTopicById(topicId);
        Question question = new Question();
        question.setContent(questionRq.getContent());
        question.setType(questionRq.getType());
        question.setDifficultyLevel(questionRq.getDifficultyLevel());
        question.setStatus(questionRq.getStatus());
        question.setTopic(topic);
        questionRepo.save(question);
        return modelMapper.map(question, Response.class);
    }

    public Response update(UpdateRequest questionRq, Long id){
        Question oldQuestion = findQuestionById(id);
        oldQuestion.setContent(questionRq.getContent());
        oldQuestion.setType(questionRq.getType());
        oldQuestion.setDifficultyLevel(questionRq.getDifficultyLevel());
        oldQuestion.setStatus(questionRq.getStatus());
        questionRepo.save(oldQuestion);
        return modelMapper.map(oldQuestion, Response.class);
    }

    public void deleted(Long id){
        Question question = findQuestionById(id);
        question.setStatus(Status.INACTIVE);
        questionRepo.save(question);
    }

    public PagedResponse<Response> getAllQuestionFromTopicId(
            Long topicId
            , int page
            , int size
            , String sortBy
            , String sortDir){

        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));

        Page<Question> questionPage = questionRepo.findAllByTopicId(topicId,pageRequest);
        if(questionPage.isEmpty()) {
            throw new QuestionException("Empty list question for topicID: " + topicId, HttpStatus.NOT_FOUND);
        }
        List<Response> responses = questionPage
                .stream()
                .map(question -> modelMapper.map(question, Response.class))
                .toList();
        return new PagedResponse<>(
                responses,
                page,
                size,
                questionPage.getTotalElements(),
                questionPage.getTotalPages(),
                questionPage.isLast()
        );

    }

}
