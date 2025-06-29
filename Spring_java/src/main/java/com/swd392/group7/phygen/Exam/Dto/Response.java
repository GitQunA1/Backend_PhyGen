package com.swd392.group7.phygen.Exam.Dto;

import com.swd392.group7.phygen.Account.Entity.Account;
import com.swd392.group7.phygen.Exam.Enum.ExamType;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import lombok.AccessLevel;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.time.LocalDateTime;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Response {

    Long id;

    boolean isDraft;

    ExamType examType;

    LocalDateTime createAt;


}
