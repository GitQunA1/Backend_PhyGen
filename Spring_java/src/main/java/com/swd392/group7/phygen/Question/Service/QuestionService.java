package com.swd392.group7.phygen.Question.Service;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.swd392.group7.phygen.AI.GeminiAI.GeminiService;
import com.swd392.group7.phygen.AI.Structure.Dto.SubjectDto;
import com.swd392.group7.phygen.Answer.Entity.Answer;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Question.Dto.QuestionResponse;
import com.swd392.group7.phygen.Question.Dto.QuestionCreateRequest;
import com.swd392.group7.phygen.Question.Dto.QuestionUpdateRequest;
import com.swd392.group7.phygen.Question.Entity.Question;
import com.swd392.group7.phygen.Question.Enum.Status;
import com.swd392.group7.phygen.Question.Exception.QuestionException;
import com.swd392.group7.phygen.Question.Repository.QuestionRepo;
import com.swd392.group7.phygen.Subject.Entity.Subject;
import com.swd392.group7.phygen.Subject.Repository.SubjectRepo;
import com.swd392.group7.phygen.Topic.Entity.Topic;
import com.swd392.group7.phygen.Topic.Service.TopicService;
import lombok.RequiredArgsConstructor;
import org.modelmapper.ModelMapper;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.util.List;

@Service
@RequiredArgsConstructor
public class QuestionService {
    private final TopicService topicService;
    private final QuestionRepo questionRepo;
    private final SubjectRepo subjectRepo;
    private final ModelMapper modelMapper;
    private final ObjectMapper objectMapper;
    private final GeminiService geminiService;

    public Question findQuestionById(Long id){
        return questionRepo.findById(id).orElseThrow(()-> new QuestionException("Cant found Question", HttpStatus.NOT_FOUND));
    }

    public QuestionResponse create(QuestionCreateRequest questionRq, Long topicId){
        Topic topic = topicService.findTopicById(topicId);
        Question question = new Question();
        question.setContent(questionRq.getContent());
        question.setType(questionRq.getType());
        question.setDifficultyLevel(questionRq.getDifficultyLevel());
        question.setStatus(questionRq.getStatus());
        question.setTopic(topic);
        questionRepo.save(question);
        return modelMapper.map(question, QuestionResponse.class);
    }

    public QuestionResponse update(QuestionUpdateRequest questionRq, Long id){
        Question oldQuestion = findQuestionById(id);
        oldQuestion.setContent(questionRq.getContent());
        oldQuestion.setType(questionRq.getType());
        oldQuestion.setDifficultyLevel(questionRq.getDifficultyLevel());
        oldQuestion.setStatus(questionRq.getStatus());
        questionRepo.save(oldQuestion);
        return modelMapper.map(oldQuestion, QuestionResponse.class);
    }

    public void deleted(Long id){
        Question question = findQuestionById(id);
        question.setStatus(Status.inactive);
        questionRepo.save(question);
    }

    public PagedResponse<QuestionResponse> getAllQuestionFromTopicId(
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
        List<QuestionResponse> responses = questionPage
                .stream()
                .map(question -> modelMapper.map(question, QuestionResponse.class))
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

    public SubjectDto insertDataFromGeminiJsonFormat(String fileUrl) throws IOException {
            String geminiJsonFormat = geminiService.formatJson(fileUrl);
            SubjectDto subjectDto = objectMapper.readValue(geminiJsonFormat, SubjectDto.class);
            System.out.println("DTO sau khi parse JSON:\n" + subjectDto);
            Subject subject = modelMapper.map(subjectDto,Subject.class);
            if(subject.getTopics() != null){
                for(Topic topic : subject.getTopics()){
                    topic.setSubject(subject);
                    if(topic.getSubTopics()!=null){
                        for(Topic subtopic : topic.getSubTopics()){
                            subtopic.setParent(topic);
                            if(subtopic.getQuestions()!=null){
                                for(Question question :subtopic.getQuestions()){
                                    question.setTopic(subtopic);
                                    if(question.getAnswers()!=null){
                                        for(Answer answer : question.getAnswers()){
                                            answer.setQuestion(question);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if(topic.getQuestions()!=null){
                        for(Question question : topic.getQuestions()){
                            question.setTopic(topic);
                            if(question.getAnswers() != null){
                                for(Answer answer : question.getAnswers()){
                                    answer.setQuestion(question);
                                }
                            }
                        }
                    }
                }
            }
            System.out.println("Entity sau khi ánh xạ và thiết lập quan hệ:\n" + subject);
            subjectRepo.save(subject);
            return modelMapper.map(subject, SubjectDto.class);
    }

}
