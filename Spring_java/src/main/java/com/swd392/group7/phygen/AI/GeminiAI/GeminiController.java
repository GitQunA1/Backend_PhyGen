package com.swd392.group7.phygen.AI.GeminiAI;

import com.swd392.group7.phygen.Common.ApiResponse;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/Gemini")
@CrossOrigin("*")
@SecurityRequirement(name="api")
public class GeminiController {

    private final GeminiService geminiService;

    @PostMapping("/generateExam")
    public ResponseEntity<?> chatWithGemini(@RequestBody GenerateExamRequest request){
        String answer = geminiService.generateExam(request);
        ApiResponse<?> response = new ApiResponse<>(true,"Here the response from chat gemini",answer,null);
        return ResponseEntity.ok(response);
    }

}
