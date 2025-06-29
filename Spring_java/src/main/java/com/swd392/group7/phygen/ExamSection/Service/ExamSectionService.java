package com.swd392.group7.phygen.ExamSection.Service;

import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Exam.Entity.Exam;
import com.swd392.group7.phygen.Exam.Exception.ExamException;
import com.swd392.group7.phygen.Exam.Service.ExamService;
import com.swd392.group7.phygen.ExamSection.Dto.CreateRequest;
import com.swd392.group7.phygen.ExamSection.Dto.Response;
import com.swd392.group7.phygen.ExamSection.Dto.UpdateRequest;
import com.swd392.group7.phygen.ExamSection.Entity.ExamSection;
import com.swd392.group7.phygen.ExamSection.Exception.ExamSectionException;
import com.swd392.group7.phygen.ExamSection.Repository.ExamSectionRepo;
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
public class ExamSectionService {
    private final ExamSectionRepo examSectionRepo;
    private final ModelMapper modelMapper;
    private final ExamService examService;

    public ExamSection findExamSectionById(Long id){
        return examSectionRepo.findById(id).orElseThrow(()->new ExamSectionException("Cant found exam section", HttpStatus.NOT_FOUND));
    }

    public Response create(CreateRequest examSectionRq, Long examId){
        Exam exam = examService.findExamById(examId);
        ExamSection examSection = new ExamSection();
        examSection.setSectionName(examSectionRq.getSectionName());
        examSection.setDisplayOrder(examSectionRq.getDisplayOrder());
        examSection.setDeleted(examSectionRq.isDeleted());
        examSection.setExam(exam);
        examSectionRepo.save(examSection);
        return modelMapper.map(examSection,Response.class);
    }

    public Response update(UpdateRequest examSectionRq, Long id){
        ExamSection oldExamSection = findExamSectionById(id);
        oldExamSection.setSectionName(examSectionRq.getSectionName());
        oldExamSection.setDisplayOrder(examSectionRq.getDisplayOrder());
        examSectionRepo.save(oldExamSection);
        return modelMapper.map(oldExamSection,Response.class);
    }

    public void deleted(Long id){
        ExamSection examSection = findExamSectionById(id);
        examSection.setDeleted(true);
        examSectionRepo.save(examSection);
    }

    public PagedResponse<Response> getAllExamSectionFromExamId(
            Long examId,
            int page,
            int size,
            String sortBy,
            String sortDir){
        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));
        Page<ExamSection> examSectionPage = examSectionRepo.findAllByExamId(examId,pageRequest);
        if(examSectionPage.isEmpty()){
            throw new ExamException("Empty list exam section for examId: "+examId,HttpStatus.NOT_FOUND);
        }
        List<Response> responses = examSectionPage.stream()
                .map(examSection -> modelMapper.map(examSection, Response.class))
                .toList();
        return new PagedResponse<>(
                responses,
                page,
                size,
                examSectionPage.getTotalElements(),
                examSectionPage.getTotalPages(),
                examSectionPage.isLast()
        );

    }
}
