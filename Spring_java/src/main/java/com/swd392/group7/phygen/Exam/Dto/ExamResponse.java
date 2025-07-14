package com.swd392.group7.phygen.Exam.Dto;

import com.swd392.group7.phygen.Exam.Enum.ExamType;
import com.swd392.group7.phygen.Exam.Enum.Status;
import lombok.AccessLevel;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.time.LocalDateTime;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ExamResponse {

    Long id;

    boolean isDraft;

    ExamType examType;

    LocalDateTime createAt;

    Status status;

}
