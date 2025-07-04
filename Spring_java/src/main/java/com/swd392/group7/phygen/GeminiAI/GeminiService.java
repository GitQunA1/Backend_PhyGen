package com.swd392.group7.phygen.GeminiAI;

import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.List;
import java.util.Map;

@Service
@RequiredArgsConstructor
public class GeminiService {

    private final RestTemplate restTemplate;

    private String apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/";

    private String model = "gemini-2.5-flash";

    @Value("${gemini_key}")
    private String GEMINI_KEY;

//    public String generateExam(List<Map<String,Object>> jsonFiles, String content){
//
//        String endpoint = apiUrl + model + "generateContent?key=" + GEMINI_KEY;
//        StringBuilder promptBuilder = new StringBuilder();
//        promptBuilder.append("Imagine you are a knowledgeable teacher across various subjects.\n");
//        promptBuilder.append("Based on the given content and the JSON files representing responses of a list of entities,\n");
//        promptBuilder.append("analyze the field names and data to identify relationships between them.\n");
//        promptBuilder.append("Use this understanding to generate a comprehensive exam in Markdown format.\n");
//        promptBuilder.append("The exam should include sections such as a title, multiple-choice questions, short answer questions, and extended response items.\n");
//        promptBuilder.append("Each question should be clearly written, relevant to the topic, and—if applicable—include correct answers or answer markings.\n");
//        promptBuilder.append("Ensure the questions reflect the concepts and relationships derived from the JSON data.\n");
//        promptBuilder.append("Generate the Markdown exam naturally and academically, grounded in the structure and content provided.\n");
//        promptBuilder.append("JSON data will be attached below (or provided through an API during implementation).\n");
//        promptBuilder.append("Content:\n").append(content).append("\n\n");
//        promptBuilder.append("JSON files:\n");
//        int idx = 1;
//        for (Map<String, Object> file : jsonFiles){
//
//        }
//
//    }

}
