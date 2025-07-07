package com.swd392.group7.phygen.AI.GeminiAI;

public record GenerateExamRequest (
    String title,
    String gradeLevel,
    String subject,
    String difficulty,
    String examType
){}

