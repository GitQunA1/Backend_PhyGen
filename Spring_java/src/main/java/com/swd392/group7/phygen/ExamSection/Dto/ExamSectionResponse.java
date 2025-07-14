package com.swd392.group7.phygen.ExamSection.Dto;

import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ExamSectionResponse {

    Long id;

    String sectionName;

    int displayOrder;

    boolean isDeleted;

}
