package com.swd392.group7.phygen.AI.GeminiAI;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;

public class PromptBuilder {

    private static final Path generateExamTemplate = Path.of("src/main/resources/examgenerate_template.txt");

    public static String buildExamPrompt(GenerateExamRequest request ){
        try{
            String template = Files.readString(generateExamTemplate);
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

}
