package com.swd392.group7.phygen.ExamVersion.Service;

import com.swd392.group7.phygen.Account.Entity.Account;
import com.swd392.group7.phygen.Account.Service.AccountService;
import com.swd392.group7.phygen.Answer.Exception.AnswerException;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Exam.Entity.Exam;
import com.swd392.group7.phygen.Exam.Service.ExamService;
import com.swd392.group7.phygen.ExamVersion.Dto.ExamVersionCreateRequest;
import com.swd392.group7.phygen.ExamVersion.Dto.ExamVersionResponse;
import com.swd392.group7.phygen.ExamVersion.Dto.ExamVersionUpdateRequest;
import com.swd392.group7.phygen.ExamVersion.Entity.ExamVersion;
import com.swd392.group7.phygen.ExamVersion.Exception.ExamVersionException;
import com.swd392.group7.phygen.ExamVersion.Repository.ExamVersionRepo;
import com.swd392.group7.phygen.Supabase.SupabaseStorageService;
import lombok.RequiredArgsConstructor;
import org.modelmapper.ModelMapper;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Sort;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.util.List;

@Service
@RequiredArgsConstructor
public class ExamVersionService {
    private final ExamVersionRepo examVersionRepo;
    private final ModelMapper modelMapper;
    private final ExamService examService;
    private final SupabaseStorageService supabaseStorageService;
    private final AccountService accountService;

    public ExamVersion findExamVersionById(Long id){
        return examVersionRepo.findById(id).orElseThrow(()-> new ExamVersionException("Cant found that ExamVersion", HttpStatus.NOT_FOUND));
    }
    public ExamVersionResponse upload(ExamVersionCreateRequest examVersionRq,Long examId, MultipartFile multipartFile ) throws IOException {
        Account account = accountService.getCurrentAccount();
        Exam exam = examService.findExamById(examId);
        ExamVersion examVersion = new ExamVersion();
        examVersion.setVersionCode(examVersionRq.getVersionCode());
        String fileName = multipartFile.getOriginalFilename();
        String key = account.getUsername() + "/" + fileName;
        supabaseStorageService.uploadFile(multipartFile,key,examVersionRq.getNameBucket());
        String pdfUrl = supabaseStorageService.getSignedUrl(key,examVersionRq.getNameBucket());
        examVersion.setPdfUrl(pdfUrl);
        examVersion.setExam(exam);
        examVersionRepo.save(examVersion);
        return modelMapper.map(examVersion, ExamVersionResponse.class);
    }

    public ExamVersionResponse update(ExamVersionUpdateRequest examVersionRq, Long id){
        ExamVersion oldExamVersion = findExamVersionById(id);
        oldExamVersion.setVersionCode(examVersionRq.getVersionCode());
        oldExamVersion.setPdfUrl(examVersionRq.getPdfUrl());
        examVersionRepo.save(oldExamVersion);
        return modelMapper.map(oldExamVersion, ExamVersionResponse.class);
    }

    public void deleted(Long id){
        ExamVersion examVersion = findExamVersionById(id);
        examVersionRepo.delete(examVersion);
    }

    public PagedResponse<ExamVersionResponse> getAllExamVersionFromExamId(
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
        List<ExamVersionResponse> respons = examVersionPage
                .stream()
                .map(examVersion -> modelMapper.map(examVersion, ExamVersionResponse.class))
                .toList();
        return new PagedResponse<>(
                respons,
                page,
                size,
                examVersionPage.getTotalElements(),
                examVersionPage.getTotalPages(),
                examVersionPage.isLast()
        );
    }



}
