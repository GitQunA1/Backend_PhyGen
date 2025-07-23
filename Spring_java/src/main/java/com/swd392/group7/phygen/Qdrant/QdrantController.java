package com.swd392.group7.phygen.Qdrant;

import com.swd392.group7.phygen.Common.ApiResponse;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/Qdrant")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class QdrantController {

    private final QdrantService qdrantService;

    public ResponseEntity<?> createCollection(@RequestParam String collectionName){
        qdrantService.createCollection(collectionName);
        ApiResponse<?> response = new ApiResponse<>(true,"Create successfully",null,null);
        return ResponseEntity.ok(response);
    }

}
