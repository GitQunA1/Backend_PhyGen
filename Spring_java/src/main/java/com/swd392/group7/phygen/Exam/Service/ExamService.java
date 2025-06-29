package com.swd392.group7.phygen.Exam.Service;

import com.swd392.group7.phygen.Account.Entity.Account;
import com.swd392.group7.phygen.Account.Repository.AccountRepo;
import com.swd392.group7.phygen.Account.Service.AccountService;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Exam.Dto.CreateRequest;
import com.swd392.group7.phygen.Exam.Dto.Response;
import com.swd392.group7.phygen.Exam.Dto.UpdateRequest;
import com.swd392.group7.phygen.Exam.Entity.Exam;
import com.swd392.group7.phygen.Exam.Exception.ExamException;
import com.swd392.group7.phygen.Exam.Repository.ExamRepo;
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
public class ExamService {
    private final AccountRepo accountRepo;
    private final AccountService accountService;
    private final ExamRepo examRepo;
    private final ModelMapper modelMapper;

    public Exam findExamById(Long id){
        return examRepo.findById(id).orElseThrow(()-> new ExamException("Cant found that Exam", HttpStatus.NOT_FOUND));
    }

    public Response create(CreateRequest examRq){
        Account account = accountService.getCurrentAccount();
        Exam exam = new Exam();
        exam.setExamType(examRq.getExamType());
        exam.setCreateAt(exam.getCreateAt());
        exam.setAccount(account);
        examRepo.save(exam);
        return modelMapper.map(exam, Response.class);
    }

    public Response update(UpdateRequest examRq, Long id){
        Exam oldExam = findExamById(id);
        oldExam.setExamType(examRq.getExamType());
        oldExam.setCreateAt((examRq.getCreateAt()));
        examRepo.save(oldExam);
        return modelMapper.map(oldExam,Response.class);
    }

    public void deleted(Long id){
        Exam exam = findExamById(id);
        exam.setDraft(true);
        examRepo.save(exam);
    }

    public PagedResponse<Response> getAllExamFromAccountId(
            int page,
            int size,
            String sortBy,
            String sortDir){
        Account account = accountService.getCurrentAccount();
        Sort.Direction direction = sortDir.equalsIgnoreCase("desc") ? Sort.Direction.DESC : Sort.Direction.ASC;
        PageRequest pageRequest = PageRequest.of(page, size, Sort.by(direction, sortBy));

        Page<Exam> examPage = examRepo.findAllByAccountId(account.getId(), pageRequest);
        if(examPage.isEmpty()){
            throw new ExamException("Empty list exam for accountId: "+account.getId(),HttpStatus.NOT_FOUND);
        }
        List<Response> responses = examPage.stream().map(exam -> modelMapper.map(exam, Response.class)).toList();
        return new PagedResponse<>(
                responses,
                page,
                size,
                examPage.getTotalElements(),
                examPage.getTotalPages(),
                examPage.isLast()
        );
    }
}
