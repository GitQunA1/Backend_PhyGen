package com.swd392.group7.phygen.Exam.Entity;

import com.swd392.group7.phygen.Account.Entity.Account;
import com.swd392.group7.phygen.Exam.Enum.Status;
import com.swd392.group7.phygen.ExamSection.Entity.ExamSection;
import com.swd392.group7.phygen.ExamVersion.Entity.ExamVersion;
import com.swd392.group7.phygen.Exam.Enum.ExamType;
import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.time.LocalDateTime;
import java.util.List;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Exam {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    Long id;

    boolean isDraft = true;

    @Enumerated(EnumType.STRING)
    Status status;

    @Enumerated(EnumType.STRING)
    @Column(name = "examtype")
    ExamType examType;


    LocalDateTime createdAt;

    @Column(name = "exam_json")
    String json;

    @ManyToOne
    @JoinColumn(name = "created_by")
    Account account;

    @OneToMany(mappedBy = "exam",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<ExamSection> examSections;

    @OneToMany(mappedBy = "exam",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<ExamVersion> examVersions;

}
