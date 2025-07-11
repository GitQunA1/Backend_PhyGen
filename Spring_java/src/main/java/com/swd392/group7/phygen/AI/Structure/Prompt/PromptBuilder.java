package com.swd392.group7.phygen.AI.Structure.Prompt;

import com.swd392.group7.phygen.AI.Structure.Dto.GenerateExamRequest;
import org.springframework.core.io.ClassPathResource;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.nio.charset.StandardCharsets;

@Component
public class PromptBuilder {

    private final String generateExamTemplatePath = "examgenerate_template.txt";
    private final String formatExamTemplatePath = "structureJsonOCR_Gemini_template.txt";
    private final String schemaPath = "schema.json";

    public String readSchemaJson() {
        try {
            return readTemplate(schemaPath);
        } catch (IOException e) {
            throw new RuntimeException("❌ Không thể đọc file schema.json từ resources: " + schemaPath, e);
        }
    }


    public String buildGenerateExamPrompt(GenerateExamRequest request ){
        try{
            String template = readTemplate(generateExamTemplatePath);
            return template
                    .replace("{{title}}", request.title())
                    .replace("{{gradeLevel}}", request.gradeLevel())
                    .replace("{{subject}}", request.subject())
                    .replace("{{difficulty}}", request.difficulty())
                    .replace("{{examType}}", request.examType());
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
