package com.swd392.group7.phygen.Common;

import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api")
@CrossOrigin("*")
@SecurityRequirement(name = "api")
public class PingController {

    @GetMapping("/ping")
    public ResponseEntity<?> keepSwaggerAwake(){
        return ResponseEntity.ok("swagger awake");
    }

}
