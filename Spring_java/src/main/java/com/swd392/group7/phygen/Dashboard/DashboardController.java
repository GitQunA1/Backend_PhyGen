package com.swd392.group7.phygen.Dashboard;

import com.swd392.group7.phygen.Common.ApiResponse;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Map;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/dashboard")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class DashboardController {

    private final DashboardService dashboardService;

    @GetMapping("/exams/monthly-counts")
    public ResponseEntity<?> totalGenerateExam(@RequestParam int targetYear){
        Map<Integer,Long> total = dashboardService.totalGenerateExam(targetYear);
        ApiResponse<Map<Integer,Long>> response = new ApiResponse<>(true,"total generated exam in year "+targetYear,total,null);
        return ResponseEntity.ok(response);
    }


}
