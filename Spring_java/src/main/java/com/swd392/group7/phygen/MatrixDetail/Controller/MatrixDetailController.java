package com.swd392.group7.phygen.MatrixDetail.Controller;

import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.MatrixDetail.Enum.DifficultyLevel;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.lang.reflect.Array;
import java.util.Arrays;
import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/matrixDetail")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class MatrixDetailController {


    @GetMapping("/difficultyLevel")
    public ResponseEntity<?> getAllDifficultyLevel(){
        List<String> difficultyLevel = Arrays.stream(DifficultyLevel.values())
                .map(Enum::name)
                .toList();
        ApiResponse<List<String>> response = new ApiResponse<>(true,"Print enum difficulty level",difficultyLevel,null);
        return ResponseEntity.ok(response);

    }

}
