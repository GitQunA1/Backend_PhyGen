package com.swd392.group7.phygen.Exam.Dto;

import com.swd392.group7.phygen.ExamMatrix.Enum.ExamType;
import com.swd392.group7.phygen.MatrixDetail.Enum.DifficultyLevel;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ExamStructureRequest {
    ExamType examType;
    Long subjectId;
    DifficultyLevel difficultLevel;
}
