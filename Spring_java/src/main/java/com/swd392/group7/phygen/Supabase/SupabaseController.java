package com.swd392.group7.phygen.Supabase;

import com.swd392.group7.phygen.Common.ApiResponse;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/Supabase")
@CrossOrigin("*")
@SecurityRequirement(name="api")
public class SupabaseController {

    private final SupabaseStorageService supabaseStorageService;

    @PostMapping(value = "/upload_storage", consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public ResponseEntity<?> uploadFile(@RequestParam("file") MultipartFile file)throws IOException {
        String nameBucket = "swd392phygen.docs";
        String key = file.getOriginalFilename();
        String result = supabaseStorageService.uploadFile(file,key,nameBucket);
        ApiResponse<?> response = new ApiResponse<>(true,"Upload file to supabase success",result,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/getSignUrl")
    public ResponseEntity<?> getUrlFile(@RequestParam("fileName") String fileName){
        String nameBucket = "swd392phygen.docs";
        String url = supabaseStorageService.getSignedUrl(fileName,nameBucket);
        ApiResponse<?> response = new ApiResponse<>(true,"Get signed Url successfully",url,null);
        return ResponseEntity.ok(response);
    }


}
