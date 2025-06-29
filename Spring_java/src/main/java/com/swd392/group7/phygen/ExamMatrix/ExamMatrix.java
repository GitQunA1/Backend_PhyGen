//package com.swd392.group7.phygen.ExamMatrix;
//
//import com.swd392.group7.phygen.Account.Entity.Account;
//import com.swd392.group7.phygen.ExamMatrix.Enum.ExamType;
//import com.swd392.group7.phygen.ExamMatrix.Enum.Status;
//import com.swd392.group7.phygen.MatrixSection.MatrixSection;
//import com.swd392.group7.phygen.Subject.Entity.Subject;
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
//public class ExamMatrix {
//    @Id
//    @GeneratedValue(strategy = GenerationType.AUTO)
//    Long id;
//
//    @Enumerated(EnumType.STRING)
//    ExamType examType;
//
//    @Enumerated(EnumType.STRING)
//    Status status;
//
//    @ManyToOne
//    @JoinColumn(name = "created_by")
//    Account account;
//
//    @ManyToOne
//    @JoinColumn(name = "subject_id")
//    Subject subject;
//
//    @OneToMany(mappedBy = "examMatrix",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
//    List<MatrixSection> matrixSections;
//}
