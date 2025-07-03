package com.swd392.group7.phygen.MistralAI;

import com.google.api.client.util.Value;
import lombok.RequiredArgsConstructor;
import org.springframework.http.*;
import org.springframework.stereotype.Service;
import org.springframework.web.client.HttpClientErrorException;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.util.Base64;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

@Service
@RequiredArgsConstructor
public class MistralAIService {

    private final RestTemplate restTemplate;

    @Value("${mistral.api.key}")
    private String apiKey;

    public String importFile(MultipartFile multipartFile){
        try{
            // 1. Encode file thành base64 dạng Data URL
            String mimeType = multipartFile.getContentType();
            String base64 = Base64.getEncoder().encodeToString(multipartFile.getBytes());
            String documentUrl = "data:" + mimeType + ";base64," + base64;

            // 2. Body tối giản: chỉ gồm model + document
            Map<String, Object> document = Map.of(
                    "type", "document_url",
                    "document_url", documentUrl
            );

            Map<String, Object> payload = Map.of(
                    "model", "mistral-ocr-latest",
                    "document", document
            );

            // 3. Header
            HttpHeaders headers = new HttpHeaders();
            headers.setContentType(MediaType.APPLICATION_JSON);
            headers.setBearerAuth(apiKey); // inject từ config

            // 4. Request + call API
            HttpEntity<Map<String, Object>> request = new HttpEntity<>(payload, headers);
            String endpoint = "https://api.mistral.ai/v1/ocr/process";
            ResponseEntity<String> response = restTemplate.postForEntity(endpoint, request, String.class);

            return response.getBody();

        }catch (IOException e) {
            throw new MistralAIException("Failed to read file content", HttpStatus.BAD_REQUEST);
        } catch (HttpClientErrorException e) {
            throw new MistralAIException("Mistral OCR API error: " + e.getResponseBodyAsString(),HttpStatus.UNPROCESSABLE_ENTITY);
        } catch (Exception e) {
            throw new MistralAIException("Unexpected error occurred", HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }
}
