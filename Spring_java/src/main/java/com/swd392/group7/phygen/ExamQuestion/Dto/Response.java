package com.swd392.group7.phygen.ExamQuestion.Dto;

import com.swd392.group7.phygen.ExamSection.Entity.ExamSection;
import com.swd392.group7.phygen.Question.Entity.Question;
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

    int orderIndex;

}
