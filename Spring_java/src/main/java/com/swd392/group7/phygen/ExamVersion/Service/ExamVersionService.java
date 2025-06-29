package com.swd392.group7.phygen.ExamVersion.Service;

import com.swd392.group7.phygen.Answer.Exception.AnswerException;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Exam.Entity.Exam;
import com.swd392.group7.phygen.Exam.Service.ExamService;
import com.swd392.group7.phygen.ExamVersion.Dto.CreateRequest;
import com.swd392.group7.phygen.ExamVersion.Dto.Response;
import com.swd392.group7.phygen.ExamVersion.Dto.UpdateRequest;
import com.swd392.group7.phygen.ExamVersion.Entity.ExamVersion;
import com.swd392.group7.phygen.ExamVersion.Exception.ExamVersionException;
import com.swd392.group7.phygen.ExamVersion.Repository.ExamVersionRepo;
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
public class ExamVersionService {
    private final ExamVersionRepo examVersionRepo;
    private final ModelMapper modelMapper;
    private final ExamService examService;

    public ExamVersion findExamVersionById(Long id){
        return examVersionRepo.findById(id).orElseThrow(()-> new ExamVersionException("Cant found that ExamVersion", HttpStatus.NOT_FOUND));
    }
    public Response create(CreateRequest examVersionRq,Long examId){
        Exam exam = examService.findExamById(examId);
        ExamVersion examVersion = new ExamVersion();
        examVersion.setVersionCode(examVersionRq.getVersionCode());
        examVersion.setPdfUrl(examVersionRq.getPdfUrl());
        examVersion.setQuestionOrder(examVersionRq.getQuestionOrder());
        examVersion.setExam(exam);
        examVersionRepo.save(examVersion);
        return modelMapper.map(examVersion, Response.class);
    }

    public Response update(UpdateRequest examVersionRq,Long id){
        ExamVersion oldExamVersion = findExamVersionById(id);
        oldExamVersion.setVersionCode(examVersionRq.getVersionCode());
        oldExamVersion.setPdfUrl(examVersionRq.getPdfUrl());
        oldExamVersion.setQuestionOrder(examVersionRq.getQuestionOrder());
        examVersionRepo.save(oldExamVersion);
        return modelMapper.map(oldExamVersion, Response.class);
    }

    public void deleted(Long id){
        ExamVersion examVersion = findExamVersionById(id);
        examVersionRepo.delete(examVersion);
    }

    public PagedResponse<Response> getAllExamVersionFromExamId(
            Long examId,
            int page,
            int size,
            String sortBy,
            String sortDir
    ){
        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));

        Page<ExamVersion> examVersionPage = examVersionRepo.findAllByExamId(examId,pageRequest);
        if(examVersionPage.isEmpty()){
            throw new AnswerException("Empty list examVersion for examId: "+examId,HttpStatus.NOT_FOUND);
        }
        List<Response> responses = examVersionPage
                .stream()
                .map(examVersion -> modelMapper.map(examVersion, Response.class))
                .toList();
        return new PagedResponse<>(
                responses,
                page,
                size,
                examVersionPage.getTotalElements(),
                examVersionPage.getTotalPages(),
                examVersionPage.isLast()
        );
    }
}
