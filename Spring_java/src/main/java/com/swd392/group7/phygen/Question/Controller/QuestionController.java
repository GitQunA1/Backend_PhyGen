package com.swd392.group7.phygen.Question.Controller;

import com.swd392.group7.phygen.AI.Structure.Dto.SubjectDto;
import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Question.Dto.QuestionCreateRequest;
import com.swd392.group7.phygen.Question.Dto.QuestionResponse;
import com.swd392.group7.phygen.Question.Dto.QuestionUpdateRequest;
import com.swd392.group7.phygen.Question.Dto.StatusUpdateRequest;
import com.swd392.group7.phygen.Question.Enum.Status;
import com.swd392.group7.phygen.Question.Service.QuestionService;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.io.IOException;
import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/questions")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class QuestionController {
    private final QuestionService questionService;

    @PostMapping("{topicId}")
    public ResponseEntity<?> create(@Valid @RequestBody QuestionCreateRequest questionRq, @PathVariable Long topicId){
        QuestionResponse questionRp = questionService.create(questionRq, topicId);
        ApiResponse<QuestionResponse> response = new ApiResponse<>(true,"Question created successfully",questionRp,null);
        return ResponseEntity.ok(response);
    }

    @PutMapping("{id}")
    public ResponseEntity<?> updated(@Valid @RequestBody QuestionUpdateRequest questionRq, @PathVariable Long id){
        QuestionResponse questionRp = questionService.update(questionRq,id);
        ApiResponse<QuestionResponse> response = new ApiResponse<>(true, "Question update successfully", questionRp,null);
        return ResponseEntity.ok(response);
    }

    @PutMapping("/status")
    public ResponseEntity<?> updated(@RequestBody StatusUpdateRequest statusUpdateRequest){
        List<QuestionResponse> questionRp = questionService.statusUpdate(statusUpdateRequest);
        ApiResponse<List<QuestionResponse>> response = new ApiResponse<>(true, "Question update successfully", questionRp,null);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping("{id}")
    public ResponseEntity<?> deleted(@PathVariable Long id){
        questionService.deleted(id);
        ApiResponse<QuestionResponse> response = new ApiResponse<>(true, "Question deleted successfully", null,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/by-topic/{topicId}")
    public ResponseEntity<?> getAllQuestionFromTopicId(
            @PathVariable Long topicId,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir
    ){
        PagedResponse<QuestionResponse> pagedResponse = questionService.getAllQuestionFromTopicId(topicId, page, size, sortBy, sortDir);
        ApiResponse<PagedResponse<QuestionResponse>> response = new ApiResponse<>(true,"get Question from topicId"+topicId+"successfully",pagedResponse,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping()
    public ResponseEntity<?> getAllQuestion(
            @RequestParam(defaultValue = "active")Status status,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir
    ){
        PagedResponse<QuestionResponse> pagedResponse = questionService.getAllQuestion(status,page, size, sortBy, sortDir);
        ApiResponse<PagedResponse<QuestionResponse>> response = new ApiResponse<>(true,"get Question from status "+ status +"successfully",pagedResponse,null);
        return ResponseEntity.ok(response);
    }

    @PostMapping("/ai-generations/from-url")
    public ResponseEntity<?> insertDataFromGeminiJsonFormat(@RequestParam String fileUrl,@RequestParam Long subjectId) throws IOException {
        SubjectDto subjectDto = questionService.insertDataFromGeminiJsonFormat(fileUrl,subjectId);
        ApiResponse<SubjectDto> response = new ApiResponse<>(true,"Insert successfully",subjectDto,null);
        return ResponseEntity.ok(response);
    }

}
