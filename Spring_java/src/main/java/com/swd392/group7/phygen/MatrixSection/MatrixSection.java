package com.swd392.group7.phygen.MatrixSection;

import com.swd392.group7.phygen.ExamMatrix.ExamMatrix;
import com.swd392.group7.phygen.MatrixDetail.MatrixDetail;
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
public class MatrixSection {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    Long id;

    String sectionName;

    int displayOrder;

    boolean isDeleted;

    @ManyToOne
    @JoinColumn(name = "matrix_id")
    ExamMatrix examMatrix;

    @OneToMany(mappedBy = "matrixSection",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<MatrixDetail> matrixDetails;
}
