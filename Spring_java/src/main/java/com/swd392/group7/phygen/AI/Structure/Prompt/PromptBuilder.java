package com.swd392.group7.phygen.AI.Structure.Prompt;

import org.springframework.core.io.ClassPathResource;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.nio.charset.StandardCharsets;

@Component
public class PromptBuilder {

    private final String generateExamTemplatePath = "Prompt/examgenerate_template.txt";
    private final String formatExamTemplatePath = "Prompt/structureJsonOCR_Gemini_template.txt";
    private final String schemaPath = "JsonStorage/schema.json";

    public String readSchemaJson() {
        try {
            return readTemplate(schemaPath);
        } catch (IOException e) {
            throw new RuntimeException("❌ Không thể đọc file schema.json từ resources: " + schemaPath, e);
        }
    }

    public String buildGenerateExamPrompt(){
        try{
            return readTemplate(generateExamTemplatePath);
        }catch (IOException e){
            throw new RuntimeException("Khong the doc template nay", e);
        }
    }

    public String buildFormatJsonPrompt(String document){
        try{
            String template = readTemplate(formatExamTemplatePath);
            return template.replace("{{DOCUMENT_CONTENT}}",document);
        }catch (IOException e){
            throw new RuntimeException("Khong the format tai lieu nay", e);
        }
    }

    private String readTemplate(String fileName) throws IOException {
        ClassPathResource resource = new ClassPathResource(fileName);
        return new String(resource.getInputStream().readAllBytes(), StandardCharsets.UTF_8);
    }



}
