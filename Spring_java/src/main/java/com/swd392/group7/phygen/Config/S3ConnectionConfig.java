package com.swd392.group7.phygen.Config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import software.amazon.awssdk.auth.credentials.AwsBasicCredentials;
import software.amazon.awssdk.auth.credentials.StaticCredentialsProvider;
import software.amazon.awssdk.http.urlconnection.UrlConnectionHttpClient;
import software.amazon.awssdk.regions.Region;
import software.amazon.awssdk.services.s3.S3Client;
import software.amazon.awssdk.services.s3.S3Configuration;

import java.net.URI;

@Configuration
public class S3ConnectionConfig {

    @Value("${supabase_access_key}")
    private String SUPABASE_ACCESS_KEY;

    @Value("${supabase_secret_access_key}")
    private String SUPABASE_SECRET_ACCESS_KEY;

    @Bean
    public S3Client s3Client() {
        return S3Client.builder()
                .endpointOverride(URI.create("https://kbicikwvyuseaqrfzkqq.supabase.co/storage/v1/s3"))
                .credentialsProvider(StaticCredentialsProvider.create(AwsBasicCredentials.create(SUPABASE_ACCESS_KEY, SUPABASE_SECRET_ACCESS_KEY)))
                .region(Region.AP_SOUTHEAST_1)
                .serviceConfiguration(
                        S3Configuration
                                .builder()
                                .pathStyleAccessEnabled(true)
                                .build())
                                .httpClientBuilder(UrlConnectionHttpClient.builder()
                )
                .build();
    }
}
