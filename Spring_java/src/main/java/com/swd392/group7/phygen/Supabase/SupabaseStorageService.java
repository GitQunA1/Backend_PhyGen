package com.swd392.group7.phygen.Supabase;

import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;
import software.amazon.awssdk.core.sync.RequestBody;
import software.amazon.awssdk.services.s3.S3Client;
import software.amazon.awssdk.services.s3.model.PutObjectRequest;

import java.io.IOException;

@Service
@RequiredArgsConstructor
public class SupabaseStorageService {


    private final S3Client s3Client;

    @Value("${supabase_s3_bucket}")
    private String bucket;

    public String uploadFile(MultipartFile multipartFile, String key) throws IOException{
        s3Client.putObject(
                PutObjectRequest.builder()
                        .bucket(bucket)
                        .key(key)
                        .acl("private")
                        .build(), RequestBody.fromInputStream(multipartFile.getInputStream(), multipartFile.getSize())
        );
        return key;
    }


}
