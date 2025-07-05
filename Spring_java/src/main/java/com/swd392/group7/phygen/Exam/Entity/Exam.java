package com.swd392.group7.phygen.Exam.Entity;

import com.swd392.group7.phygen.Account.Entity.Account;
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
    @GeneratedValue(strategy = GenerationType.AUTO)
    Long id;

    boolean isDraft = false;

    @Enumerated(EnumType.STRING)
    @Column(name = "examtype")
    ExamType examType;


    LocalDateTime createdAt;

    @ManyToOne
    @JoinColumn(name = "created_by")
    Account account;

    @OneToMany(mappedBy = "exam",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<ExamSection> examSections;

    @OneToMany(mappedBy = "exam",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<ExamVersion> examVersions;

}
