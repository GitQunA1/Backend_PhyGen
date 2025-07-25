package com.swd392.group7.phygen.Exam.Dto;

import com.swd392.group7.phygen.ExamMatrix.Dto.ExamMatrixStructureResponse;
import com.swd392.group7.phygen.Subject.Dto.SubjectStructureResponse;
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
public class ExamStructureResponse {
    SubjectStructureResponse subjectStructureResponse;
}
