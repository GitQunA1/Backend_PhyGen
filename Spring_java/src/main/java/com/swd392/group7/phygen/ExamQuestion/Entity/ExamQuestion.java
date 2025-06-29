package com.swd392.group7.phygen.ExamQuestion.Entity;


import com.swd392.group7.phygen.ExamSection.Entity.ExamSection;
import com.swd392.group7.phygen.Question.Entity.Question;
import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ExamQuestion {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    Long id;

    int orderIndex;

    @ManyToOne
    @JoinColumn(name = "section_id")
    ExamSection examSection;

    @ManyToOne
    @JoinColumn(name = "question_id")
    Question question;
}
