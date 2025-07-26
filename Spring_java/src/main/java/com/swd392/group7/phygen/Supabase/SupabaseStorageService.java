package com.swd392.group7.phygen.Supabase;

import lombok.RequiredArgsConstructor;
import org.apache.tika.Tika;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;
import software.amazon.awssdk.core.sync.RequestBody;
import software.amazon.awssdk.services.s3.S3Client;
import software.amazon.awssdk.services.s3.model.*;
import software.amazon.awssdk.services.s3.presigner.S3Presigner;
import software.amazon.awssdk.services.s3.presigner.model.GetObjectPresignRequest;

import java.io.IOException;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.time.Duration;
import java.util.List;
import java.util.Objects;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class SupabaseStorageService {


    private final S3Client s3Client;

    private final S3Presigner s3Presigner;

    public String detectContentType(MultipartFile file) throws IOException {
        Tika tika = new Tika();
        return tika.detect(file.getInputStream());
    }

    public String uploadFile(MultipartFile multipartFile, String key,String bucket) throws IOException{
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

    public String getSignedUrl(String key, String bucket){
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

    public List<FileDto> listFileKeysInBucket(){
        String bucketName = "swd392phygen.docs";
        ListObjectsV2Request listObjectsV2Request = ListObjectsV2Request.builder()
                .bucket(bucketName)
                .build();
        ListObjectsV2Response listObjectsV2Response = s3Client.listObjectsV2(listObjectsV2Request);
        return listObjectsV2Response.contents().stream()
                .map(s3Object -> {
                    String key = s3Object.key();
                    if (key.endsWith(".emptyFolderPlaceholder") || key.endsWith("/")) {
                        return null;
                    }
                    String url = getSignedUrl(key, bucketName); // Tạo URL
                    return new FileDto(key, url);
                })
                .filter(Objects::nonNull)
                .collect(Collectors.toList());
    }

    public List<FileEntryDto> listFileKeysAndCreationDatesInBucket() {
        String bucketName = "swd392phygen.exam";
        ListObjectsV2Request listObjectsV2Request = ListObjectsV2Request.builder()
                .bucket(bucketName)
                .build();

        ListObjectsV2Response listObjectsV2Response = s3Client.listObjectsV2(listObjectsV2Request);

        return listObjectsV2Response.contents().stream()
                .filter(s3Object -> {
                    String key = s3Object.key();
                    return !key.endsWith(".emptyFolderPlaceholder") && !key.endsWith("/");
                })
                .map(s3Object -> new FileEntryDto(s3Object.key(), s3Object.lastModified()))
                .collect(Collectors.toList());
    }

    public List<S3Object> listFilesInBucket(String bucketName) {
        ListObjectsV2Request listObjectsV2Request = ListObjectsV2Request.builder()
                .bucket(bucketName)
                .build();

        ListObjectsV2Response listObjectsV2Response = s3Client.listObjectsV2(listObjectsV2Request);

        return listObjectsV2Response.contents();
    }

    public boolean deleted(String bucketName, String fileName){
        try{
            DeleteObjectRequest deletedObject = DeleteObjectRequest
                    .builder()
                    .bucket(bucketName)
                    .key(fileName)
                    .build();
            DeleteObjectResponse deleteObjectResponse = s3Client.deleteObject(deletedObject);
            System.out.println("File '" + fileName + "' deleted successfully from bucket '" + bucketName + "'.");
            return true;

        }catch(S3Exception s3Exception){
            System.err.println("Error deleting file '" + fileName + "' from bucket '" + bucketName + "': " + s3Exception.getMessage());
            return false;
        }
    }

    public boolean checkUrlExpired(String signedUrl){
        HttpURLConnection connection = null;
        try {
            URL url = new URL(signedUrl);
            connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("HEAD");
            connection.setConnectTimeout(5000);
            connection.setReadTimeout(5000);

            int responseCode = connection.getResponseCode();
            System.out.println("Kiểm tra URL: " + signedUrl + " -> Mã phản hồi: " + responseCode);
            if (responseCode >= 200 && responseCode < 300) {
                return false;
            }
            else return responseCode != 403;
        } catch (IOException e) {
            System.err.println("Lỗi khi kiểm tra URL " + signedUrl + ": " + e.getMessage());
            return true;
        } finally {
            if (connection != null) {
                connection.disconnect();
            }
        }
    }

    public String[] extractBucketAndPath(String urlString) {
        try {
            String path = new URL(urlString).getPath();
            int s3Index = path.indexOf("/s3/");

            if (s3Index == -1) {
                return new String[]{"", ""};
            }

            String relevantPath = path.substring(s3Index + "/s3/".length());
            int firstSlashIndex = relevantPath.indexOf('/');

            String bucketName = (firstSlashIndex == -1) ? relevantPath : relevantPath.substring(0, firstSlashIndex);
            String folderPathAndFileName = (firstSlashIndex == -1) ? "" : relevantPath.substring(firstSlashIndex + 1);

            return new String[]{bucketName, folderPathAndFileName};

        } catch (MalformedURLException e) {
            return new String[]{"", ""}; // URL không hợp lệ, trả về rỗng
        }
    }


}
