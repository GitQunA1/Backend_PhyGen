package com.swd392.group7.phygen.Subject.Controller;

import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Subject.Dto.SubjectResponse;
import com.swd392.group7.phygen.Subject.Service.SubjectService;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/subjects")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class SubjectController {

    private final SubjectService subjectService;

    @GetMapping()
    public ResponseEntity<?> getAll(){
        List<SubjectResponse> subjectResponses = subjectService.getAll();
        ApiResponse<List<SubjectResponse>> response = new ApiResponse<>(true,"Get all name and grade",subjectResponses,null);
        return ResponseEntity.ok(response);
    }
}
