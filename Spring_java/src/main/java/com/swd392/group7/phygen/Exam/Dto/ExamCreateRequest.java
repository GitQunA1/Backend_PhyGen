package com.swd392.group7.phygen.Exam.Dto;

import com.swd392.group7.phygen.ExamMatrix.Enum.ExamType;
import com.swd392.group7.phygen.MatrixDetail.Enum.DifficultyLevel;
import lombok.*;
import lombok.experimental.FieldDefaults;


@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ExamCreateRequest {

    Long subjectId;

    ExamType examType;

    DifficultyLevel difficultyLevel;

    String examQuantity;
}
