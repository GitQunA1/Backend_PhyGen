package com.swd392.group7.phygen.MistralAI;

import com.swd392.group7.phygen.Common.ApiResponse;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/OCR")
@CrossOrigin("*")
@SecurityRequirement(name="api")
public class MistralAIController {
    private final MistralAIService mistralAIService;


    @PostMapping(value = "/upload_url")
    public ResponseEntity<?> uploadFile(@RequestParam("url") String url) {
        String result = mistralAIService.importDocUrl(url);
        ApiResponse<?> response = new ApiResponse<>(true,"Read file",result,null);
        return ResponseEntity.ok(response);
    }

}
