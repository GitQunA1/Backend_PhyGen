package com.swd392.group7.phygen.AI.MistralAI;

import jakarta.annotation.PostConstruct;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.*;
import org.springframework.stereotype.Service;
import org.springframework.web.client.HttpClientErrorException;
import org.springframework.web.client.RestTemplate;

import java.util.Map;

@Service
@RequiredArgsConstructor
public class MistralAIService {

    private final RestTemplate restTemplate;

    @Value("${mistral_api_key}")
    private String MISTRAL_KEY;

    @PostConstruct
    public void init() {
        System.out.println("mistral api key = " + MISTRAL_KEY);
    }


    public String importDocUrl(String fileUrl) {
        try {
            Map<String, Object> document = Map.of(
                    "type", "document_url",
                    "document_url", fileUrl
            );

            Map<String, Object> payload = Map.of(
                    "model", "mistral-ocr-latest",
                    "document", document
            );

            HttpHeaders headers = new HttpHeaders();
            headers.setContentType(MediaType.APPLICATION_JSON);
            headers.setBearerAuth(MISTRAL_KEY);

            HttpEntity<Map<String, Object>> request = new HttpEntity<>(payload, headers);
            String endpoint = "https://api.mistral.ai/v1/ocr";
            ResponseEntity<String> response = restTemplate.postForEntity(endpoint, request, String.class);

            return response.getBody();
        } catch (HttpClientErrorException e) {
            throw new MistralAIException("Mistral OCR API error: " + e.getResponseBodyAsString(), HttpStatus.UNPROCESSABLE_ENTITY);
        } catch (Exception e) {
            throw new MistralAIException("Unexpected error occurred: " + e.getMessage(), HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }
}
