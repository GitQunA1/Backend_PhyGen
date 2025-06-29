package com.swd392.group7.phygen.ExamVersion.Controller;

import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.ExamVersion.Dto.CreateRequest;
import com.swd392.group7.phygen.ExamVersion.Dto.Response;
import com.swd392.group7.phygen.ExamVersion.Dto.UpdateRequest;
import com.swd392.group7.phygen.ExamVersion.Service.ExamVersionService;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/examVersion")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class ExamVersionController {
    private final ExamVersionService examVersionService;

    @PostMapping("/exam/{examId}")
    public ResponseEntity<?> create(@Valid @RequestBody CreateRequest examVersionRq,@PathVariable Long examId){
        Response examVersionRp = examVersionService.create(examVersionRq, examId);
        ApiResponse<Response> response = new ApiResponse<>(true,"Create ExamVersion successfully",examVersionRp,null);
        return ResponseEntity.ok(response);
    }

    @PutMapping("{id}")
    public ResponseEntity<?> update(@Valid @RequestBody UpdateRequest examVersionRq,@PathVariable Long id){
        Response examVersionRp = examVersionService.update(examVersionRq, id);
        ApiResponse<Response> response = new ApiResponse<>(true,"Update ExamVersion successfully",examVersionRp,null);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping("{id}")
    public ResponseEntity<?> deleted(@PathVariable Long id){
        examVersionService.deleted(id);
        ApiResponse<Response> response = new ApiResponse<>(true,"Deleted ExamVersion successfully",null,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/getAllExamVersionFromExamId/{examId}")
    public ResponseEntity<?> getAllExamVersionFromExamId(
            @PathVariable Long examId,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir
    ){
        PagedResponse<Response> pagedResponse = examVersionService.getAllExamVersionFromExamId(examId,page,size,sortBy,sortDir);
        ApiResponse<PagedResponse<Response>> response = new ApiResponse<>(true,"Get ExamVersion form that examId: "+ examId,pagedResponse,null);
        return ResponseEntity.ok(response);
    }
}
