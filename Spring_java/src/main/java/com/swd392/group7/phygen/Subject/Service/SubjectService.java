package com.swd392.group7.phygen.Subject.Service;

import com.swd392.group7.phygen.Subject.Dto.SubjectResponse;
import com.swd392.group7.phygen.Subject.Entity.Subject;
import com.swd392.group7.phygen.Subject.Enum.Grade;
import com.swd392.group7.phygen.Subject.Exception.SubjectException;
import com.swd392.group7.phygen.Subject.Repository.SubjectRepo;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
public class SubjectService {

    private final SubjectRepo subjectRepo;

    public Subject findSubjectById(Long id){
        return subjectRepo.findById(id).orElseThrow(()-> new SubjectException("cant found that subject", HttpStatus.NOT_FOUND));
    }

    public List<SubjectResponse> getAll(){
        List<Subject> subjects = subjectRepo.findAll();
        List<SubjectResponse> responses = new ArrayList<>();
        for (Subject subject : subjects){
            responses.add(new SubjectResponse(subject.getName(), subject.getGrade()));
        }
        return responses;
    }
}
