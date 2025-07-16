package com.swd392.group7.phygen.ExamMatrix.Controller;

import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.ExamMatrix.Dto.ExamMatrixEnumResponse;
import com.swd392.group7.phygen.ExamMatrix.Service.ExamMatrixService;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/exam-matrices")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class ExamMatrixController {
    private final ExamMatrixService examMatrixService;

    @GetMapping("/by-subject/{subjectId}")
    public ResponseEntity<?> showExamType(@PathVariable Long subjectId){
        List<ExamMatrixEnumResponse> examMatrixEnumResponses = examMatrixService.showExamType(subjectId);
        ApiResponse<List<ExamMatrixEnumResponse>> response = new ApiResponse<>(true,"Show list ExamType",examMatrixEnumResponses,null);
        return ResponseEntity.ok(response);
    }

}
