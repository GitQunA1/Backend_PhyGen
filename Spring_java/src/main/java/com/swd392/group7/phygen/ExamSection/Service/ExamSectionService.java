package com.swd392.group7.phygen.ExamSection.Service;

import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Exam.Entity.Exam;
import com.swd392.group7.phygen.Exam.Exception.ExamException;
import com.swd392.group7.phygen.Exam.Repository.ExamRepo;
import com.swd392.group7.phygen.Exam.Service.ExamService;
import com.swd392.group7.phygen.ExamSection.Dto.ExamSectionCreateRequest;
import com.swd392.group7.phygen.ExamSection.Dto.ExamSectionResponse;
import com.swd392.group7.phygen.ExamSection.Dto.ExamSectionUpdateRequest;
import com.swd392.group7.phygen.ExamSection.Entity.ExamSection;
import com.swd392.group7.phygen.ExamSection.Exception.ExamSectionException;
import com.swd392.group7.phygen.ExamSection.Repository.ExamSectionRepo;
import lombok.RequiredArgsConstructor;
import org.modelmapper.ModelMapper;
import org.springframework.context.annotation.Lazy;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
public class ExamSectionService {
    private final ExamSectionRepo examSectionRepo;
    private final ModelMapper modelMapper;
    private final ExamRepo examRepo;

    public ExamSection findExamSectionById(Long id){
        return examSectionRepo.findById(id).orElseThrow(()->new ExamSectionException("Cant found exam section", HttpStatus.NOT_FOUND));
    }

    public List<ExamSectionResponse> create(ExamSectionCreateRequest examSectionRq, Long examId){
        Exam exam = examRepo.findById(examId).orElseThrow(()-> new ExamException("Cant found exam", HttpStatus.NOT_FOUND));
        List<ExamSectionResponse> createdResponses = new ArrayList<>();
        for(int i = 0; i < examSectionRq.getSectionName().size(); i++ ){
            ExamSection examSection = new ExamSection();
            examSection.setSectionName(examSectionRq.getSectionName().get(i));
            examSection.setDisplayOrder(i+1);
            examSection.setDeleted(false);
            examSection.setExam(exam);
            ExamSection saveSection = examSectionRepo.save(examSection);
            createdResponses.add(modelMapper.map(saveSection,ExamSectionResponse.class));
        }
        return createdResponses;
    }

    public ExamSectionResponse update(ExamSectionUpdateRequest examSectionRq, Long id){
        ExamSection oldExamSection = findExamSectionById(id);
        oldExamSection.setSectionName(examSectionRq.getSectionName());
        oldExamSection.setDisplayOrder(examSectionRq.getDisplayOrder());
        examSectionRepo.save(oldExamSection);
        return modelMapper.map(oldExamSection, ExamSectionResponse.class);
    }

    public void deleted(Long id){
        ExamSection examSection = findExamSectionById(id);
        examSection.setDeleted(true);
        examSectionRepo.save(examSection);
    }

    public PagedResponse<ExamSectionResponse> getAllExamSectionFromExamId(
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
        List<ExamSectionResponse> respons = examSectionPage.stream()
                .map(examSection -> modelMapper.map(examSection, ExamSectionResponse.class))
                .toList();
        return new PagedResponse<>(
                respons,
                page,
                size,
                examSectionPage.getTotalElements(),
                examSectionPage.getTotalPages(),
                examSectionPage.isLast()
        );

    }
}
