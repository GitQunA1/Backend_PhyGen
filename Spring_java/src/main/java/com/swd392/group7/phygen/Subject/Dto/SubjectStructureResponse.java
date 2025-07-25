package com.swd392.group7.phygen.Subject.Dto;

import com.swd392.group7.phygen.ExamMatrix.Dto.ExamMatrixStructureResponse;
import com.swd392.group7.phygen.Subject.Enum.Grade;
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
public class SubjectStructureResponse {
    Long id;

    String name;

    Grade grade;

    ExamMatrixStructureResponse examMatrixStructureResponse;
}
