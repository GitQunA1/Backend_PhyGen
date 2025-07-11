package com.swd392.group7.phygen.AI.Structure.Dto;

public record GenerateExamRequest (
    String title,
    String gradeLevel,
    String subject,
    String difficulty,
    String examType
){}

