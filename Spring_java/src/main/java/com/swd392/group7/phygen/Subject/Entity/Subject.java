package com.swd392.group7.phygen.Subject.Entity;

import com.swd392.group7.phygen.ExamMatrix.ExamMatrix;
//import com.swd392.group7.phygen.Subject.Converter.GradeConverter;
import com.swd392.group7.phygen.Subject.Enum.Grade;
import com.swd392.group7.phygen.Topic.Entity.Topic;
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
public class Subject {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    Long id;
    String name;
    String description;

//    @Convert(converter = GradeConverter.class)
    @Enumerated(EnumType.STRING)
    Grade grade;

    boolean isDeleted;

    @OneToMany(mappedBy = "subject",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<ExamMatrix> examMatrices;

    @OneToMany(mappedBy = "subject",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<Topic> topics;
}
