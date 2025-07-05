package com.swd392.group7.phygen.Config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import dev.langchain4j.model.googleai.GoogleAiGeminiChatModel;

@Configuration
public class GeminiConfig {


    @Value("${gemini_key}")
    private String GEMINI_KEY;

    @Bean
    public GoogleAiGeminiChatModel geminiChatModel(){
        return GoogleAiGeminiChatModel.builder()
                .apiKey(GEMINI_KEY)
                .modelName("gemini-2.5-flash")
                .temperature(0.7)
                .build();
    }

}
