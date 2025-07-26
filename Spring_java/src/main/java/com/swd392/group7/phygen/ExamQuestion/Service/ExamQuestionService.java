package com.swd392.group7.phygen.ExamQuestion.Service;

import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.ExamQuestion.Dto.ExamQuestionCreateRequest;
import com.swd392.group7.phygen.ExamQuestion.Dto.ExamQuestionResponse;
import com.swd392.group7.phygen.ExamQuestion.Dto.ExamQuestionUpdateRequest;
import com.swd392.group7.phygen.ExamQuestion.Entity.ExamQuestion;
import com.swd392.group7.phygen.ExamQuestion.Exception.ExamQuestionException;
import com.swd392.group7.phygen.ExamQuestion.Repository.ExamQuestionRepo;
import com.swd392.group7.phygen.ExamSection.Entity.ExamSection;
import com.swd392.group7.phygen.ExamSection.Exception.ExamSectionException;
import com.swd392.group7.phygen.ExamSection.Service.ExamSectionService;
import com.swd392.group7.phygen.Question.Entity.Question;
import com.swd392.group7.phygen.Question.Service.QuestionService;
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
public class ExamQuestionService {

    private final QuestionService questionService;
    private final ModelMapper modelMapper;
    private final ExamQuestionRepo examQuestionRepo;

    private final ExamSectionService examSectionService;

    public ExamQuestion findExamQuestionById(Long id){
        return examQuestionRepo.findById(id).orElseThrow(()->new ExamQuestionException("Cant found exam question", HttpStatus.NOT_FOUND));
    }

    public ExamQuestionResponse create(ExamQuestionCreateRequest examQuestionRq, Long questionId, Long examSectionId) {
        Question question = questionService.findQuestionById(questionId);
        ExamSection examSection = examSectionService.findExamSectionById(examSectionId);
        ExamQuestion examQuestion = new ExamQuestion();
        examQuestion.setOrderIndex(examQuestionRq.getOrderIndex());
        examQuestion.setQuestion(question);
        examQuestion.setExamSection(examSection);
        examQuestionRepo.save(examQuestion);
        return modelMapper.map(examQuestion, ExamQuestionResponse.class);
    }

    public ExamQuestionResponse update(ExamQuestionUpdateRequest examQuestionRq, Long id){
        ExamQuestion oldExamQuestion = findExamQuestionById(id);
        oldExamQuestion.setOrderIndex(examQuestionRq.getOrderIndex());
        examQuestionRepo.save(oldExamQuestion);
        return modelMapper.map(oldExamQuestion, ExamQuestionResponse.class);
    }

    public PagedResponse<ExamQuestionResponse> getAllExamQuestionFromQuestionId(
            Long questionId
            , int page
            , int size
            , String sortBy
            , String sortDir
    ){
        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));
        Page<ExamQuestion> examQuestionPage = examQuestionRepo.findAllByQuestionId(questionId,pageRequest);
        if(examQuestionPage.isEmpty()){
            throw new ExamSectionException("Cant found list examQuestion from QuestionId:"+ questionId,HttpStatus.NOT_FOUND);
        }
        List<ExamQuestionResponse> respons = examQuestionPage
                .stream()
                .map(examQuestion -> modelMapper.map(examQuestion, ExamQuestionResponse.class))
                .toList();
        return new PagedResponse<>(
                respons,
                page,
                size,
                examQuestionPage.getTotalElements(),
                examQuestionPage.getTotalPages(),
                examQuestionPage.isLast());
    }

    public PagedResponse<ExamQuestionResponse> getAllExamQuestionFromExamSectionId(
            Long examSectionId
            , int page
            , int size
            , String sortBy
            , String sortDir
    ){
        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));
        Page<ExamQuestion> examSectionPage =  examQuestionRepo.findAllByExamSectionId(examSectionId,pageRequest);
        if(examSectionPage.isEmpty()){
            throw new ExamSectionException("Cant found list examQuestion from ExamSectionId: "+ examSectionId,HttpStatus.NOT_FOUND);
        }
        List<ExamQuestionResponse> respons = examSectionPage
                .stream()
                .map(examSection -> modelMapper.map(examSection, ExamQuestionResponse.class))
                .toList();
        return new PagedResponse<>(
                respons,
                page,
                size,
                examSectionPage.getTotalElements(),
                examSectionPage.getTotalPages(),
                examSectionPage.isLast());
    }

    public PagedResponse<ExamQuestionResponse> getAllExamQuestion(
              int page
            , int size
            , String sortBy
            , String sortDir
    ){
        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));
        Page<ExamQuestion> examSectionPage =  examQuestionRepo.findAll(pageRequest);
        if(examSectionPage.isEmpty()){
            throw new ExamSectionException("Cant found list examQuestion ",HttpStatus.NOT_FOUND);
        }
        List<ExamQuestionResponse> response = examSectionPage
                .stream()
                .map(examSection -> modelMapper.map(examSection, ExamQuestionResponse.class))
                .toList();
        return new PagedResponse<>(
                response,
                page,
                size,
                examSectionPage.getTotalElements(),
                examSectionPage.getTotalPages(),
                examSectionPage.isLast());
    }

}
