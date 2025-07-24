package com.swd392.group7.phygen.Question.Service;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.swd392.group7.phygen.AI.GeminiAI.GeminiService;
import com.swd392.group7.phygen.AI.Structure.Dto.AnswerDto;
import com.swd392.group7.phygen.AI.Structure.Dto.QuestionDto;
import com.swd392.group7.phygen.AI.Structure.Dto.SubjectDto;
import com.swd392.group7.phygen.AI.Structure.Dto.TopicDto;
import com.swd392.group7.phygen.Answer.Entity.Answer;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Question.Dto.QuestionResponse;
import com.swd392.group7.phygen.Question.Dto.QuestionCreateRequest;
import com.swd392.group7.phygen.Question.Dto.QuestionUpdateRequest;
import com.swd392.group7.phygen.Question.Dto.StatusUpdateRequest;
import com.swd392.group7.phygen.Question.Entity.Question;
import com.swd392.group7.phygen.Question.Enum.Status;
import com.swd392.group7.phygen.Question.Exception.QuestionException;
import com.swd392.group7.phygen.Question.Repository.QuestionRepo;
import com.swd392.group7.phygen.Subject.Entity.Subject;
import com.swd392.group7.phygen.Subject.Repository.SubjectRepo;
import com.swd392.group7.phygen.Subject.Service.SubjectService;
import com.swd392.group7.phygen.Topic.Entity.Topic;
import com.swd392.group7.phygen.Topic.Repository.TopicRepo;
import com.swd392.group7.phygen.Topic.Service.TopicService;
import lombok.RequiredArgsConstructor;
import org.hibernate.Hibernate;
import org.modelmapper.ModelMapper;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.io.IOException;
import java.util.ArrayList;
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
    private final SubjectService subjectService;
    private final TopicRepo topicRepo;

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

    public List<QuestionResponse> statusUpdate(StatusUpdateRequest statusRq){
        List<Question> questions = questionRepo.findAllById(statusRq.getId());
        for (Question question : questions) {
            question.setStatus(statusRq.getStatus());
        }
        questionRepo.saveAll(questions);
        List<QuestionResponse> questionResponses = new ArrayList<>();
        for (Question question : questions) {
            QuestionResponse response = modelMapper.map(question, QuestionResponse.class);
            questionResponses.add(response);
        }
        return questionResponses;
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
            , String sortDir) {

        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));

        Page<Question> questionPage = questionRepo.findAllByTopicId(topicId, pageRequest);
        if (questionPage.isEmpty()) {
            throw new QuestionException("Empty list question for topicID: " + topicId, HttpStatus.NOT_FOUND);
        }
        List<QuestionResponse> responses = questionPage
                .stream()
                .map(question -> modelMapper.map(question, QuestionResponse.class))
                .filter(questionResponse -> questionResponse.getStatus() == Status.active)
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

        public PagedResponse<QuestionResponse> getAllQuestion(
                Status status,
                int page,
                int size,
                String sortBy,
                String sortDir){

            Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
            PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));

            Page<Question> questionPage = questionRepo.findAllByStatus(pageRequest,status);
            if(questionPage.isEmpty()) {
                throw new QuestionException("Empty list question", HttpStatus.NOT_FOUND);
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

    @Transactional
    public SubjectDto insertDataFromGeminiJsonFormat(String fileUrl,Long subjectId) throws IOException {
            String geminiJsonFormat = geminiService.formatJson(fileUrl,subjectId);
            System.out.println("JSON received by ObjectMapper:\n" + geminiJsonFormat);
            SubjectDto subjectDto = objectMapper.readValue(geminiJsonFormat, SubjectDto.class);
            System.out.println("DTO sau khi parse JSON:\n" + subjectDto);
            Subject subject = subjectService.findSubjectById(subjectId);
        for (TopicDto topicDto : subjectDto.getTopics()) {
            Topic foundTopic = topicRepo.findByNameAndLevel(topicDto.getName(), topicDto.getLevel());

            if (foundTopic == null) {
               continue;
            }
            if (topicDto.getQuestions() != null) {
                if (foundTopic.getQuestions() == null) {
                    foundTopic.setQuestions(new ArrayList<>());
                }
                for (QuestionDto questionDto : topicDto.getQuestions()) {
                    Question newQuestion = modelMapper.map(questionDto, Question.class);
                    newQuestion.setTopic(foundTopic);

                    if (questionDto.getAnswers() != null) {
                        if (newQuestion.getAnswers() == null) {
                            newQuestion.setAnswers(new ArrayList<>());
                        }
                        for (AnswerDto answerDto : questionDto.getAnswers()) {
                            Answer newAnswer = modelMapper.map(answerDto, Answer.class);
                            newAnswer.setQuestion(newQuestion);
                            newQuestion.getAnswers().add(newAnswer);
                        }
                    }
                    foundTopic.getQuestions().add(newQuestion);
                }
            }
            if (topicDto.getSubTopics() != null) {
                if (foundTopic.getSubTopics() == null) {
                    foundTopic.setSubTopics(new ArrayList<>());
                }
                for (TopicDto subTopicDto : topicDto.getSubTopics()) {
                    Topic foundSubTopic = topicRepo.findByNameAndLevelAndParent(subTopicDto.getName(), subTopicDto.getLevel(), foundTopic);
                    if (foundSubTopic == null) {
                        continue;
                    }
                    if (subTopicDto.getQuestions() != null) {
                        if (foundSubTopic.getQuestions() == null) {
                            foundSubTopic.setQuestions(new ArrayList<>());
                        }
                        for (QuestionDto questionDto : subTopicDto.getQuestions()) {
                            Question newQuestion = modelMapper.map(questionDto, Question.class);
                            newQuestion.setTopic(foundSubTopic);
                            if (questionDto.getAnswers() != null) {
                                if (newQuestion.getAnswers() == null) {
                                    newQuestion.setAnswers(new ArrayList<>());
                                }
                                for (AnswerDto answerDto : questionDto.getAnswers()) {
                                    Answer newAnswer = modelMapper.map(answerDto, Answer.class);
                                    newAnswer.setQuestion(newQuestion);
                                    newQuestion.getAnswers().add(newAnswer);
                                }
                            }
                            foundSubTopic.getQuestions().add(newQuestion);
                        }
                    }
                }
            }
        }
        System.out.println("Entity sau khi ánh xạ và thiết lập quan hệ:\n" + subject);
        subjectRepo.save(subject);
        Hibernate.initialize(subject.getTopics());
        for (Topic topic : subject.getTopics()) {
            Hibernate.initialize(topic.getQuestions());
            Hibernate.initialize(topic.getSubTopics());
            for (Topic subTopic : topic.getSubTopics()) {
                Hibernate.initialize(subTopic.getQuestions());
                for (Question question : subTopic.getQuestions()) {
                    Hibernate.initialize(question.getAnswers());
                }
            }
            for (Question question : topic.getQuestions()) {
                Hibernate.initialize(question.getAnswers());
            }
        }
        return modelMapper.map(subject, SubjectDto.class);
    }

}
