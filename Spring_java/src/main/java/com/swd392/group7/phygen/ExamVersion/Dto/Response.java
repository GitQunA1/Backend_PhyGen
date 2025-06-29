package com.swd392.group7.phygen.ExamVersion.Dto;

import com.swd392.group7.phygen.Exam.Entity.Exam;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Response {

    Long id;

    String versionCode;

    String pdfUrl;

    String questionOrder;
}
