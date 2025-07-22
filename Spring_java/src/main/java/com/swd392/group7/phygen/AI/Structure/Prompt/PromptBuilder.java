package com.swd392.group7.phygen.AI.Structure.Prompt;

import org.springframework.core.io.ClassPathResource;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.List;

@Component
public class PromptBuilder {

    private final String generateExamTemplatePath = "Prompt/examgenerate_template.txt";
    private final String formatExamTemplatePath = "Prompt/structureJsonOCR_Gemini_template.txt";
    private final String schemaPath = "JsonStorage/schema.json";
    private final String answerUserTemplatePath = "Prompt/answerUser_template.txt";

    public String readSchemaJson() {
        try {
            return readTemplate(schemaPath);
        } catch (IOException e) {
            throw new RuntimeException("❌ Không thể đọc file schema.json từ resources: " + schemaPath, e);
        }
    }

    public String chatWithGeminiPrompt(String userName,String text){
        try{
            String template = readTemplate(answerUserTemplatePath);
            return template.replace("{{USER_NAME}}",userName)
                    .replace("{{USER_INPUT}}",text);
        }catch(IOException e){
            throw new RuntimeException("❌ Không thể đọc file schema.json từ resources: " );
        }
    }

    public String buildGenerateExamPrompt(){
        try{
            return readTemplate(generateExamTemplatePath);
        }catch (IOException e){
            throw new RuntimeException("Khong the doc template nay", e);
        }
    }

    public String buildFormatJsonPrompt(String document, String topicStructureJson){
        try{
            String template = readTemplate(formatExamTemplatePath);
            return template.replace("{{DOCUMENT_CONTENT}}",document)
                    .replace("{{TOPIC_STRUCTURE_CONTENT}}",topicStructureJson);
        }catch (IOException e){
            throw new RuntimeException("Khong the format tai lieu nay", e);
        }
    }

    private String readTemplate(String fileName) throws IOException {
        ClassPathResource resource = new ClassPathResource(fileName);
        return new String(resource.getInputStream().readAllBytes(), StandardCharsets.UTF_8);
    }



}
