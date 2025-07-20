package com.swd392.group7.phygen.Topic.Controller;

import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Topic.Dto.TopicGeminiPromptResponse;
import com.swd392.group7.phygen.Topic.Service.TopicService;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/topics")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class TopicController {

    private final TopicService topicService;

    @GetMapping("/by-subject/{subjectId}")
    public ResponseEntity<?> getAllTopicFromSubject(@PathVariable Long subjectId){
        List<TopicGeminiPromptResponse> topicGeminiPromptResponseList = topicService.topicStructureGeminiPrompt(subjectId);
        ApiResponse<List<TopicGeminiPromptResponse>> response = new ApiResponse<>(true,"get structure successfully",topicGeminiPromptResponseList,null);
        return ResponseEntity.ok(response);
    }

}
