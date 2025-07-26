package com.swd392.group7.phygen.ExamSection.Dto;

import com.swd392.group7.phygen.Question.Dto.QuestionStructureResponse;
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
public class ExamSectionStructureRequest {
    String sectionName;

    int displayOrder;

    List<QuestionStructureResponse> questions;

}
