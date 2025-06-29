package com.swd392.group7.phygen.ExamVersion.Dto;

import com.swd392.group7.phygen.Exam.Entity.Exam;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class CreateRequest {
    String versionCode;

    String pdfUrl;

    String questionOrder;
}
