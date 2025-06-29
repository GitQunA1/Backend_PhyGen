package com.swd392.group7.phygen.Answer.Controller;

import com.swd392.group7.phygen.Answer.Dto.CreateRequest;
import com.swd392.group7.phygen.Answer.Dto.Response;
import com.swd392.group7.phygen.Answer.Dto.UpdateRequest;
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
@RequestMapping("/api/answer")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class AnswerController {

    private final AnswerService answerService;

    @PostMapping("{questionId}")
    public ResponseEntity<?> create(@Valid @RequestBody CreateRequest answerRq, @PathVariable Long questionId) {
        Response answerRp = answerService.createAnswer(answerRq, questionId);
        ApiResponse<Response> response = new ApiResponse<>(true, "Answer created successfully", answerRp, null);
        return ResponseEntity.ok(response);
    }

    @PutMapping("{id}")
    public ResponseEntity<?> update(@Valid @RequestBody UpdateRequest answerRq,@PathVariable Long id){
        Response answerRp = answerService.updateAnswer(answerRq, id);
        ApiResponse<Response> response = new ApiResponse<>(true,"Answer updated successfully",answerRp,null);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping("{examId}")
    public ResponseEntity<?> deleted(@PathVariable Long examId){
        answerService.deleted(examId);
        ApiResponse<Response> response = new ApiResponse<>(true, "Answer deleted successfully",null,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("{questionId}")
    public ResponseEntity<?> getAllAnswerFromQuestionId(
            @PathVariable Long questionId,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir){
        PagedResponse<Response> pagedResponse = answerService.getAllAnswerFromQuestionId(questionId, page, size, sortBy, sortDir);
        ApiResponse<PagedResponse<Response>> response = new ApiResponse<>(true,"get Answer from questionId"+questionId+"successfully",pagedResponse,null);
        return ResponseEntity.ok(response);
    }


}
