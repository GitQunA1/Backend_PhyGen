package com.swd392.group7.phygen.Exam.Dto;

import com.swd392.group7.phygen.ExamMatrix.Enum.ExamType;
import com.swd392.group7.phygen.ExamSection.Dto.ExamSectionStructureRequest;
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
public class ExamStructureRequest {
    Long subjectId;

    String subjectName;

    ExamType examType;

    List<ExamSectionStructureRequest> sections;

    String error;
}
