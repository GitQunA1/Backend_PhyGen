package com.swd392.group7.phygen.ExamMatrix.Service;

import com.swd392.group7.phygen.ExamMatrix.Dto.ExamMatrixEnumResponse;
import com.swd392.group7.phygen.ExamMatrix.Entity.ExamMatrix;
import com.swd392.group7.phygen.ExamMatrix.Exception.ExamMatrixException;
import com.swd392.group7.phygen.ExamMatrix.Repository.ExamMatrixRepo;
import com.swd392.group7.phygen.Subject.Entity.Subject;
import com.swd392.group7.phygen.Subject.Service.SubjectService;
import lombok.RequiredArgsConstructor;
import org.modelmapper.ModelMapper;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class ExamMatrixService {
    private final ExamMatrixRepo examMatrixRepo;
    private final ModelMapper modelMapper;

    public List<ExamMatrixEnumResponse> showExamType(Long subjectId){
        List<ExamMatrix> examMatrices = examMatrixRepo.findAllBySubjectId(subjectId).orElseThrow(()-> new ExamMatrixException("Empty list exam matrix", HttpStatus.NOT_FOUND));
        return examMatrices.stream()
                .map(examMatrix -> modelMapper.map(examMatrix, ExamMatrixEnumResponse.class))
                .toList();
    }


}
