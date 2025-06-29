package com.swd392.group7.phygen.Exam.Controller;

import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.Exam.Dto.CreateRequest;
import com.swd392.group7.phygen.Exam.Dto.Response;
import com.swd392.group7.phygen.Exam.Dto.UpdateRequest;
import com.swd392.group7.phygen.Exam.Service.ExamService;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/exam")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class ExamController {
    private final ExamService examService;

    @PostMapping
    public ResponseEntity<?> create(@Valid @RequestBody CreateRequest examRq){
        Response examRp = examService.create(examRq);
        ApiResponse<Response> response = new ApiResponse<>(true,"Create Exam successfully",examRp,null);
        return ResponseEntity.ok(response);
    }
    @PutMapping("{id}")
    public ResponseEntity<?> update(@Valid @RequestBody UpdateRequest examRq,@PathVariable Long id){
        Response examRp = examService.update(examRq,id);
        ApiResponse<Response> response = new ApiResponse<>(true,"Update Exam successfully",examRp,null);
        return ResponseEntity.ok(response);
    }
    @DeleteMapping("{id}")
    public ResponseEntity<?> deleted(@PathVariable Long id){
        examService.deleted(id);
        ApiResponse<Response> response = new ApiResponse<>(true, "Deleted Exam successfully",null,null);
        return ResponseEntity.ok(response);
    }
    @GetMapping("/getAllExamFromCurrentAccount")
    public ResponseEntity<?> getAllExamFromAccountId(
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir
    ){
        PagedResponse<Response> pagedResponse = examService.getAllExamFromAccountId(page,size,sortBy,sortDir);
        ApiResponse<PagedResponse<Response>> response = new ApiResponse<>(true,"get Exam from account successfully",pagedResponse,null );
        return ResponseEntity.ok(response);
    }
}
