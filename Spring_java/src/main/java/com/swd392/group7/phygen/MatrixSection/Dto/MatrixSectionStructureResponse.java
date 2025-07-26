package com.swd392.group7.phygen.MatrixSection.Dto;

import com.swd392.group7.phygen.MatrixDetail.Dto.MatrixDetailStructureResponse;
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
public class MatrixSectionStructureResponse {
    String sectionName;
    int displayOrder;
    List<MatrixDetailStructureResponse> matrixDetailStructureResponseList;
}
