package com.swd392.group7.phygen.ExamSection.Controller;

import com.swd392.group7.phygen.Common.ApiResponse;
import com.swd392.group7.phygen.Common.PagedResponse;
import com.swd392.group7.phygen.ExamSection.Dto.CreateRequest;
import com.swd392.group7.phygen.ExamSection.Dto.Response;
import com.swd392.group7.phygen.ExamSection.Dto.UpdateRequest;
import com.swd392.group7.phygen.ExamSection.Service.ExamSectionService;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/examSection")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class ExamSectionController {

    private final ExamSectionService examSectionService;


    @PostMapping("/exam/{examId}")
    public ResponseEntity<?> create(@Valid @RequestBody CreateRequest examSectionRq,@PathVariable Long examId){
        Response examSectionRp = examSectionService.create(examSectionRq, examId);
        ApiResponse<Response> response = new ApiResponse<>(true,"Create exam section successfully",examSectionRp,null);
        return ResponseEntity.ok(response);
    }

    @PutMapping("{id}")
    public ResponseEntity<?> update(@Valid @RequestBody UpdateRequest examSectionRq,@PathVariable Long id){
        Response examSectionRp  = examSectionService.update(examSectionRq, id);
        ApiResponse<Response> response = new ApiResponse<>(true, "Create exam Section successfully",examSectionRp,null);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping("{id}")
    public ResponseEntity<?> deleted(@PathVariable Long id){
        examSectionService.deleted(id);
        ApiResponse<Response> response = new ApiResponse<>(true,"Delete examSection successfully",null,null);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/exam/{examId}")
    public ResponseEntity<?> getAllExamSectionFromThatExam(
            @PathVariable Long examId,
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "id") String sortBy,
            @RequestParam(defaultValue = "desc") String sortDir
    ){
        PagedResponse<Response> pagedResponse = examSectionService.getAllExamSectionFromExamId(examId,page,size,sortBy,sortDir);
        ApiResponse<PagedResponse<Response>> response = new ApiResponse<>(true,"Get list form examId:"+examId,pagedResponse,null);
        return ResponseEntity.ok(response);
    }

}
