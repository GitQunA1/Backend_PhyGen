//package com.swd392.group7.phygen.MatrixSection;
//
//import com.swd392.group7.phygen.ExamMatrix.ExamMatrix;
//import jakarta.persistence.*;
//import lombok.*;
//import lombok.experimental.FieldDefaults;
//
//@Entity
//@Getter
//@Setter
//@NoArgsConstructor
//@AllArgsConstructor
//@FieldDefaults(level = AccessLevel.PRIVATE)
//public class MatrixSection {
//    @Id
//    @GeneratedValue(strategy = GenerationType.AUTO)
//    Long id;
//
//    String sectionName;
//
//    int displayOrder;
//
//    boolean isDeleted;
//
//    @ManyToOne
//    @JoinColumn(name = "matrix_id")
//    ExamMatrix examMatrix;
//}
