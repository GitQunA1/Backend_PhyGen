package com.swd392.group7.phygen.Supabase;

import lombok.RequiredArgsConstructor;
import org.apache.tika.Tika;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;
import software.amazon.awssdk.core.sync.RequestBody;
import software.amazon.awssdk.services.s3.S3Client;
import software.amazon.awssdk.services.s3.model.GetObjectRequest;
import software.amazon.awssdk.services.s3.model.PutObjectRequest;
import software.amazon.awssdk.services.s3.presigner.S3Presigner;
import software.amazon.awssdk.services.s3.presigner.model.GetObjectPresignRequest;

import java.io.IOException;
import java.net.URL;
import java.time.Duration;
import java.util.List;

@Service
@RequiredArgsConstructor
public class SupabaseStorageService {


    private final S3Client s3Client;

    private final S3Presigner s3Presigner;

    @Value("${supabase_s3_bucket}")
    private String bucket;

    public String detectContentType(MultipartFile file) throws IOException {
        Tika tika = new Tika();
        return tika.detect(file.getInputStream());
    }

    public String uploadFile(MultipartFile multipartFile, String key) throws IOException{
        String contentType = detectContentType(multipartFile);
        s3Client.putObject(
                PutObjectRequest.builder()
                        .bucket(bucket)
                        .key(key)
                        .acl("private")
                        .contentType((contentType))
                        .build(), RequestBody.fromInputStream(multipartFile.getInputStream(), multipartFile.getSize())
        );
        return key;
    }

    public String getSignedUrl(String key){
        GetObjectRequest getObjectRequest = GetObjectRequest.builder()
                .bucket(bucket)
                .key(key)
                .responseContentDisposition("inline")
                .build();
        GetObjectPresignRequest getObjectPresignRequest = GetObjectPresignRequest.builder()
                .signatureDuration(Duration.ofDays(7))
                .getObjectRequest(getObjectRequest)
                .build();
        URL signedUrl = s3Presigner.presignGetObject(getObjectPresignRequest).url();
        return signedUrl.toString();
    }


}
