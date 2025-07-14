package com.swd392.group7.phygen.ExamVersion.Controller;

import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.ExamVersion.Dto.ExamVersionCreateRequest;
import com.swd392.group7.phygen.ExamVersion.Dto.ExamVersionResponse;
import com.swd392.group7.phygen.ExamVersion.Dto.ExamVersionUpdateRequest;
import com.swd392.group7.phygen.ExamVersion.Service.ExamVersionService;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/examVersion")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class ExamVersionController {
    private final ExamVersionService examVersionService;

    @PostMapping("/exam/{examId}")
    public ResponseEntity<?> create(@RequestParam("file") MultipartFile multipartFile,
                                    @RequestParam("versionCode") String versionCode,
                                    @PathVariable Long examId ) throws IOException {
        ExamVersionCreateRequest examVersionRq = new ExamVersionCreateRequest();
        examVersionRq.setVersionCode(versionCode);
        examVersionRq.setNameBucket("swd392phygen.exam");
        ExamVersionResponse examVersionRp = examVersionService.upload(examVersionRq, examId,multipartFile);
        ApiResponse<ExamVersionResponse> response = new ApiResponse<>(true,"Create ExamVersion successfully",examVersionRp,null);
        return ResponseEntity.ok(response);
    }

    @PutMapping("{id}")
    public ResponseEntity<?> update(@Valid @RequestBody ExamVersionUpdateRequest examVersionRq, @PathVariable Long id){
        ExamVersionResponse examVersionRp = examVersionService.update(examVersionRq, id);
        ApiResponse<ExamVersionResponse> response = new ApiResponse<>(true,"Update ExamVersion successfully",examVersionRp,null);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping("{id}")
    public ResponseEntity<?> deleted(@PathVariable Long id){
        examVersionService.deleted(id);
        ApiResponse<ExamVersionResponse> response = new ApiResponse<>(true,"Deleted ExamVersion successfully",null,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/getAllExamVersionFromExamId/{examId}")
    public ResponseEntity<?> getAllExamVersionFromExamId(
            @PathVariable Long examId,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir
    ){
        PagedResponse<ExamVersionResponse> pagedResponse = examVersionService.getAllExamVersionFromExamId(examId,page,size,sortBy,sortDir);
        ApiResponse<PagedResponse<ExamVersionResponse>> response = new ApiResponse<>(true,"Get ExamVersion form that examId: "+ examId,pagedResponse,null);
        return ResponseEntity.ok(response);
    }
}
