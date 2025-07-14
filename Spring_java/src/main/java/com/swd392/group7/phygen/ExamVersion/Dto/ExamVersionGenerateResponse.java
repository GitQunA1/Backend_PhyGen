package com.swd392.group7.phygen.ExamVersion.Dto;


import com.swd392.group7.phygen.Exam.Dto.GeneratedExam;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

import java.util.List;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ExamVersionGenerateResponse {
    Long examId;

    List<GeneratedExam> generatedExams;

}
