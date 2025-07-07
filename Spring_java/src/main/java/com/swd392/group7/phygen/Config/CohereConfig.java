package com.swd392.group7.phygen.Config;

import com.cohere.api.Cohere;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class CohereConfig {
    @Value("${cohere_api_key)")
    private String COHERE_API_KEY;

    @Bean
    public Cohere cohereClient(){
        return Cohere.builder()
                .token(COHERE_API_KEY)
                .clientName("embedding_text")
                .build();
    }

}
