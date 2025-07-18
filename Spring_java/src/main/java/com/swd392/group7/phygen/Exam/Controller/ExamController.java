package com.swd392.group7.phygen.Exam.Controller;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Exam.Dto.*;
import com.swd392.group7.phygen.Exam.Service.ExamService;
import com.swd392.group7.phygen.ExamMatrix.Enum.ExamType;
import com.swd392.group7.phygen.ExamVersion.Dto.ExamVersionGenerateResponse;
import com.swd392.group7.phygen.MatrixDetail.Enum.DifficultyLevel;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/exams")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class ExamController {
    private final ExamService examService;

    @PostMapping
    public ResponseEntity<?> create(@Valid @RequestBody ExamCreateRequest examRq)throws JsonProcessingException {
        ExamVersionGenerateResponse examVersionGenerateResponse = examService.create(examRq);
        ApiResponse<ExamVersionGenerateResponse> response = new ApiResponse<>(true,"Generate exam successfully", examVersionGenerateResponse,null);
        return ResponseEntity.ok(response);
    }
    @PutMapping("{id}")
    public ResponseEntity<?> update(@Valid @RequestBody ExamUpdateRequest examRq, @PathVariable Long id){
        ExamResponse examRp = examService.update(examRq,id);
        ApiResponse<ExamResponse> response = new ApiResponse<>(true,"Update Exam successfully",examRp,null);
        return ResponseEntity.ok(response);
    }
    @DeleteMapping("{id}")
    public ResponseEntity<?> deleted(@PathVariable Long id){
        examService.deleted(id);
        ApiResponse<ExamResponse> response = new ApiResponse<>(true, "Deleted Exam successfully",null,null);
        return ResponseEntity.ok(response);
    }
    @GetMapping("/my")
    public ResponseEntity<?> getAllExamFromAccountId(
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir
    ){
        PagedResponse<ExamResponse> pagedResponse = examService.getAllExamFromAccountId(page,size,sortBy,sortDir);
        ApiResponse<PagedResponse<ExamResponse>> response = new ApiResponse<>(true,"get Exam from account successfully",pagedResponse,null );
        return ResponseEntity.ok(response);
    }

    @GetMapping("/structure")
    public ResponseEntity<?> getExamStructure(@RequestParam ExamType examType,
                                              @RequestParam Long subjectId,
                                              @RequestParam DifficultyLevel difficultLevel){
        ExamStructureRequest examStructureRequest = new ExamStructureRequest(examType,subjectId,difficultLevel);
        ExamStructureResponse examStructureResponse = examService.getExamStructure(examStructureRequest);
        ApiResponse<ExamStructureResponse> response = new ApiResponse<>(true,"Get Exam Structure successfully",examStructureResponse,null);
        return ResponseEntity.ok(response);
    }
}
