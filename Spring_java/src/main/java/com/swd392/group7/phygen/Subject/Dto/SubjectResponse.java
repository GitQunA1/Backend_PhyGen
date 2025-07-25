package com.swd392.group7.phygen.Subject.Dto;

import com.swd392.group7.phygen.Subject.Enum.Grade;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class SubjectResponse {
    String name;

    Grade grade;
}
