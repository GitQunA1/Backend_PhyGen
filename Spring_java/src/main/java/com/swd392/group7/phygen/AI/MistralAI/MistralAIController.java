package com.swd392.group7.phygen.AI.MistralAI;

import com.swd392.group7.phygen.Common.ApiResponse;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/OCR")
@CrossOrigin("*")
@SecurityRequirement(name="api")
public class MistralAIController {
    private final MistralAIService mistralAIService;


    @PostMapping(value = "/upload_url")
    public ResponseEntity<?> uploadDocUrl(@RequestParam("url") String url) {
        String result = mistralAIService.importDocUrl(url);
        ByteArrayResource resource = new ByteArrayResource(result.getBytes(StandardCharsets.UTF_8));
        ApiResponse<?> response = new ApiResponse<>(true,"Read file",result,null);
        return ResponseEntity.ok(response);
    }

}
