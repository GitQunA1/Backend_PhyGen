package com.swd392.group7.phygen.ExamVersion.Entity;

import com.swd392.group7.phygen.Exam.Entity.Exam;
import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ExamVersion {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    Long id;

    String versionCode;

    String pdfUrl;

    String questionOrder;

    @ManyToOne
    @JoinColumn(name="final_exam_id")
    Exam exam;
}
