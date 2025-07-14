package com.swd392.group7.phygen.ExamMatrix.Dto;

import com.swd392.group7.phygen.ExamMatrix.Enum.ExamType;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ExamMatrixEnumResponse {
    ExamType examType;
}
