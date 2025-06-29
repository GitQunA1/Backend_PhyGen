package com.swd392.group7.phygen.Exam.Dto;

import com.swd392.group7.phygen.Account.Entity.Account;
import com.swd392.group7.phygen.Exam.Enum.ExamType;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

import java.time.LocalDateTime;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class CreateRequest {

    @Enumerated(EnumType.STRING)
    ExamType examType;

    LocalDateTime createAt;

}
