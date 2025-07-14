package com.swd392.group7.phygen.AI.GeminiAI;

import com.swd392.group7.phygen.AI.Structure.Dto.GenerateExamRequest;
import com.swd392.group7.phygen.AI.Structure.Dto.SubjectDto;
import com.swd392.group7.phygen.Common.ApiResponse;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.sql.SQLException;
import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/Gemini")
@CrossOrigin("*")
@SecurityRequirement(name="api")
public class GeminiController {

    private final GeminiService geminiService;

//    @PostMapping("/generateExam")
//    public ResponseEntity<?> chatWithGemini(@RequestBody GenerateExamRequest request){
//        String answer = geminiService.generateExam(request);
//        ApiResponse<?> response = new ApiResponse<>(true,"Here the response from chat gemini",answer,null);
//        return ResponseEntity.ok(response);
//    }

    @GetMapping("/printSchemaInfo")
    public ResponseEntity<?> printSchemaInfo()throws SQLException{
        String answer = geminiService.schemaInfo();
        ApiResponse<?> response = new ApiResponse<>(true,"Schema info",answer,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/formatJson")
    public ResponseEntity<?> formatJson(@RequestParam String fileUrl){
        String formatJson = geminiService.formatJson(fileUrl);
        ApiResponse<?> response = new ApiResponse<>(true,"Format thanh cong",formatJson,null);
        return ResponseEntity.ok(response);
    }

}
