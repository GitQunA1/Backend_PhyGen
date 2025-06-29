package com.swd392.group7.phygen.Question.Controller;

import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Question.Dto.CreateRequest;
import com.swd392.group7.phygen.Question.Dto.Response;
import com.swd392.group7.phygen.Question.Dto.UpdateRequest;
import com.swd392.group7.phygen.Question.Service.QuestionService;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/question")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class QuestionController {
    private final QuestionService questionService;

    @PostMapping("{topicId}")
    public ResponseEntity<?> create(@Valid @RequestBody CreateRequest questionRq,@PathVariable Long topicId){
        Response questionRp = questionService.create(questionRq, topicId);
        ApiResponse<Response> response = new ApiResponse<>(true,"Question created successfully",questionRp,null);
        return ResponseEntity.ok(response);
    }

    @PutMapping("{id}")
    public ResponseEntity<?> updated(@Valid @RequestBody UpdateRequest questionRq,@PathVariable Long id){
        Response questionRp = questionService.update(questionRq,id);
        ApiResponse<Response> response = new ApiResponse<>(true, "Question update successfully", questionRp,null);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping("{id}")
    public ResponseEntity<?> deleted(@PathVariable Long id){
        questionService.deleted(id);
        ApiResponse<Response> response = new ApiResponse<>(true, "Question deleted successfully", null,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("{topicId}")
    public ResponseEntity<?> getAllQuestionFromTopicId(
            @PathVariable Long topicId,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir
    ){
        PagedResponse<Response> pagedResponse = questionService.getAllQuestionFromTopicId(topicId, page, size, sortBy, sortDir);
        ApiResponse<PagedResponse<Response>> response = new ApiResponse<>(true,"get Question from topicId"+topicId+"successfully",pagedResponse,null);
        return ResponseEntity.ok(response);
    }

}
