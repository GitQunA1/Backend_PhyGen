package com.swd392.group7.phygen.ExamSection.Dto;

import com.swd392.group7.phygen.Exam.Entity.Exam;
import com.swd392.group7.phygen.ExamQuestion.Entity.ExamQuestion;
import com.swd392.group7.phygen.ExamSection.Entity.ExamSection;
import com.swd392.group7.phygen.Question.Entity.Question;
import jakarta.persistence.*;
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
public class Response {

    Long id;

    String sectionName;

    int displayOrder;

    boolean isDeleted;

}
