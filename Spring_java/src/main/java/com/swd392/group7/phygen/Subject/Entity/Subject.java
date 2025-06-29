//package com.swd392.group7.phygen.Subject.Entity;
//
//import com.swd392.group7.phygen.Subject.Converter.GradeConverter;
//import com.swd392.group7.phygen.Subject.Enum.Grade;
//import jakarta.persistence.*;
//import lombok.*;
//import lombok.experimental.FieldDefaults;
//
//import java.util.List;
//
//@Entity
//@Getter
//@Setter
//@NoArgsConstructor
//@AllArgsConstructor
//@FieldDefaults(level = AccessLevel.PRIVATE)
//public class Subject {
//    @Id
//    @GeneratedValue(strategy = GenerationType.AUTO)
//    Long id;
//    String name;
//    String description;
//
//    @Convert(converter = GradeConverter.class)
//    Grade grade;
//
//    boolean isDeleted;
//
//    @OneToMany(mappedBy = "subject",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
//    List<ExamMatrix> examMatrices;
//}
