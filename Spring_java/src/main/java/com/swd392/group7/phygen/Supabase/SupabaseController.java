package com.swd392.group7.phygen.Supabase;

import com.swd392.group7.phygen.Common.ApiResponse;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/Supabase")
@CrossOrigin("*")
@SecurityRequirement(name="api")
public class SupabaseController {

    private final SupabaseStorageService supabaseStorageService;

    @PostMapping(value = "/files", consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public ResponseEntity<?> uploadFile(@RequestParam("file") MultipartFile file,@RequestParam String key)throws IOException {
        String nameBucket = "swd392phygen.docs";
        String result = supabaseStorageService.uploadFile(file,key,nameBucket);
        ApiResponse<?> response = new ApiResponse<>(true,"Upload file to supabase success",result,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/files/{fileName}/signed-url")
    public ResponseEntity<?> getUrlFile(@RequestParam("fileName") String fileName){
        String nameBucket = "swd392phygen.docs";
        String url = supabaseStorageService.getSignedUrl(fileName,nameBucket);
        ApiResponse<?> response = new ApiResponse<>(true,"Get signed Url successfully",url,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/file-names")
    public ResponseEntity<?> listFileKeysInBucket(){
        List<FileDto> list = supabaseStorageService.listFileKeysInBucket();
        ApiResponse<List<FileDto>> response = new ApiResponse<>(true,"Get list file name successfully",list,null);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping("/fileName")
    public ResponseEntity<?> deletedFileInBucket(@RequestParam String fileName){
        String bucketName = "swd392phygen.docs";
        Boolean deleted = supabaseStorageService.deleted(bucketName,fileName);
        ApiResponse<Boolean> response = new ApiResponse<>(true,"Delete "+fileName+" successfully",deleted,null);
        return ResponseEntity.ok(response);
    }


}
