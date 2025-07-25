package com.swd392.group7.phygen.AI.Structure.Dto;

import com.swd392.group7.phygen.Subject.Enum.Grade;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
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
public class SubjectDto {
    String name;
    @Enumerated(EnumType.STRING)
    Grade grade;
    List<TopicDto> topics;
}
