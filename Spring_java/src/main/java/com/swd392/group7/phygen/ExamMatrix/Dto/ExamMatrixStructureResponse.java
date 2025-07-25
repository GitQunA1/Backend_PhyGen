package com.swd392.group7.phygen.ExamMatrix.Dto;

import com.swd392.group7.phygen.ExamMatrix.Enum.ExamType;
import com.swd392.group7.phygen.MatrixSection.Dto.MatrixSectionStructureResponse;
import com.swd392.group7.phygen.MatrixSection.MatrixSection;
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
public class ExamMatrixStructureResponse {

    ExamType examType;

    List<MatrixSectionStructureResponse> matrixSectionStructureResponses;
}
