package com.swd392.group7.phygen.ExamQuestion.Controller;


import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.ExamQuestion.Dto.ExamQuestionCreateRequest;
import com.swd392.group7.phygen.ExamQuestion.Dto.ExamQuestionResponse;
import com.swd392.group7.phygen.ExamQuestion.Dto.ExamQuestionUpdateRequest;
import com.swd392.group7.phygen.ExamQuestion.Service.ExamQuestionService;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/examQuestion")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class ExamQuestionController {

    private final ExamQuestionService examQuestionService;

    @PostMapping("/exam-section/{examSectionId}/questions/{questionId}")
    public ResponseEntity<?> create(@Valid @RequestBody ExamQuestionCreateRequest examQuestionRq, @PathVariable Long questionId, @PathVariable Long examSectionId){
        ExamQuestionResponse examQuestionRp = examQuestionService.create(examQuestionRq, questionId,examSectionId);
        ApiResponse<ExamQuestionResponse> response = new ApiResponse<>(true,"Create Exam Question successfully",examQuestionRp,null);
        return ResponseEntity.ok(response);
    }

    @PutMapping("{id}")
    public ResponseEntity<?> update(@Valid @RequestBody ExamQuestionUpdateRequest examQuestionRq, @PathVariable Long id){
        ExamQuestionResponse examQuestionRp = examQuestionService.update(examQuestionRq, id);
        ApiResponse<ExamQuestionResponse> response = new ApiResponse<>(true, "Update Exam Question successfully", examQuestionRp, null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/question/{questionId}")
    public ResponseEntity<?> getAllExamQuestionFromQuestionId(
            @PathVariable Long questionId,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir
    ){
        PagedResponse<ExamQuestionResponse> pagedResponse = examQuestionService.getAllExamQuestionFromQuestionId(questionId, page, size, sortBy, sortDir);
        ApiResponse<PagedResponse<ExamQuestionResponse>> response = new ApiResponse<>(true,"get Question from questionId"+questionId+"successfully",pagedResponse,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/exam-section/{examSectionId}")
    public ResponseEntity<?> getAllExamQuestionFromExamSectionId(
            @PathVariable Long examSectionId,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir
    ){
        PagedResponse<ExamQuestionResponse> pagedResponse = examQuestionService.getAllExamQuestionFromExamSectionId(examSectionId, page, size, sortBy, sortDir);
        ApiResponse<PagedResponse<ExamQuestionResponse>> response = new ApiResponse<>(true,"get Question from examSectionId"+examSectionId+"successfully",pagedResponse,null);
        return ResponseEntity.ok(response);
    }
}
