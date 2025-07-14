package com.swd392.group7.phygen.Answer.Service;

import com.swd392.group7.phygen.Answer.Dto.AnswerCreateRequest;
import com.swd392.group7.phygen.Answer.Dto.AnswerResponse;
import com.swd392.group7.phygen.Answer.Dto.AnswerUpdateRequest;
import com.swd392.group7.phygen.Answer.Entity.Answer;
import com.swd392.group7.phygen.Answer.Exception.AnswerException;
import com.swd392.group7.phygen.Answer.Repository.AnswerRepo;
import com.swd392.group7.phygen.Common.PagedResponse;
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
public class AnswerService {
    private final ModelMapper modelMapper;
    private final AnswerRepo answerRepo;
    private final QuestionService questionService;

    public Answer findAnswerById(Long id){
        return answerRepo.findById(id).orElseThrow(()-> new AnswerException("Cant found answer",HttpStatus.NOT_FOUND));
    }

    public AnswerResponse createAnswer(AnswerCreateRequest answerCreateRequest, Long questionId){
        Question question = questionService.findQuestionById(questionId);
        Answer answer =new Answer();
        answer.setContent(answerCreateRequest.getContent());
        answer.setCorrect(answerCreateRequest.isCorrect());
        answer.setQuestion(question);
        answerRepo.save(answer);
        return modelMapper.map(answer, AnswerResponse.class);
    }

    public AnswerResponse updateAnswer(AnswerUpdateRequest answerUpdateRequest, Long id){
        Answer oldAnswer = findAnswerById(id);
        oldAnswer.setContent(answerUpdateRequest.getContent());
        oldAnswer.setCorrect(answerUpdateRequest.isCorrect());
        answerRepo.save(oldAnswer);
        return modelMapper.map(oldAnswer, AnswerResponse.class);
    }

    public void deleted (Long examId){
        Answer answerDeleted = findAnswerById(examId);
        answerDeleted.setDeleted(true);
        answerRepo.save(answerDeleted);
    }

    public PagedResponse<AnswerResponse> getAllAnswerFromQuestionId(Long questionId
            , int page
            , int size
            , String sortBy
            , String sortDir){
        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));

        Page<Answer> answerPage = answerRepo.findAllByQuestionId(questionId,pageRequest);
        if(answerPage.isEmpty()){
            throw new AnswerException("Empty list answer for questionID: "+ questionId,HttpStatus.NOT_FOUND);
        }

        List<AnswerResponse> respons =answerPage.stream()
                .map(answer -> modelMapper.map(answer, AnswerResponse.class))
                .toList();

        return new PagedResponse<>(
                respons,
                page,
                size,
                answerPage.getTotalElements(),
                answerPage.getTotalPages(),
                answerPage.isLast()
        );
    }


}
