package com.swd392.group7.phygen.Answer.Controller;

import com.swd392.group7.phygen.Answer.Dto.AnswerCreateRequest;
import com.swd392.group7.phygen.Answer.Dto.AnswerResponse;
import com.swd392.group7.phygen.Answer.Dto.AnswerUpdateRequest;
import com.swd392.group7.phygen.Answer.Service.AnswerService;
import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Common.PagedResponse;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/answers")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class AnswerController {

    private final AnswerService answerService;

    @PostMapping("/by-question/{questionId}")
    public ResponseEntity<?> create(@Valid @RequestBody AnswerCreateRequest answerRq, @PathVariable Long questionId) {
        AnswerResponse answerRp = answerService.createAnswer(answerRq, questionId);
        ApiResponse<AnswerResponse> response = new ApiResponse<>(true, "Answer created successfully", answerRp, null);
        return ResponseEntity.ok(response);
    }

    @PutMapping("{id}")
    public ResponseEntity<?> update(@Valid @RequestBody AnswerUpdateRequest answerRq, @PathVariable Long id){
        AnswerResponse answerRp = answerService.updateAnswer(answerRq, id);
        ApiResponse<AnswerResponse> response = new ApiResponse<>(true,"Answer updated successfully",answerRp,null);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping("/by-exam/{examId}")
    public ResponseEntity<?> deleted(@PathVariable Long examId){
        answerService.deleted(examId);
        ApiResponse<AnswerResponse> response = new ApiResponse<>(true, "Answer deleted successfully",null,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/by-question/{questionId}")
    public ResponseEntity<?> getAllAnswerFromQuestionId(
            @PathVariable Long questionId,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir){
        PagedResponse<AnswerResponse> pagedResponse = answerService.getAllAnswerFromQuestionId(questionId, page, size, sortBy, sortDir);
        ApiResponse<PagedResponse<AnswerResponse>> response = new ApiResponse<>(true,"get Answer from questionId"+questionId+"successfully",pagedResponse,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping()
    public ResponseEntity<?> getAllAnswer(
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir){
        PagedResponse<AnswerResponse> pagedResponse = answerService.getAllAnswer(page, size, sortBy, sortDir);
        ApiResponse<PagedResponse<AnswerResponse>> response = new ApiResponse<>(true,"get list answer successfully",pagedResponse,null);
        return ResponseEntity.ok(response);
    }


}
