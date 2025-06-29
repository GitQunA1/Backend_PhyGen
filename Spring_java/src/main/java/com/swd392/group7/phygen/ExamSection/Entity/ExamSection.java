package com.swd392.group7.phygen.ExamSection.Entity;

import com.swd392.group7.phygen.Exam.Entity.Exam;
import com.swd392.group7.phygen.ExamQuestion.Entity.ExamQuestion;
import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.util.List;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ExamSection {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    Long id;

    String sectionName;

    int displayOrder;

    boolean isDeleted;

    @ManyToOne
    @JoinColumn(name = "exam_id")
    Exam exam;

    @OneToMany(mappedBy = "examSection",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<ExamQuestion> examQuestions;

}
