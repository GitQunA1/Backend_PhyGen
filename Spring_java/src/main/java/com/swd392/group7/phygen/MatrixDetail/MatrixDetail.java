package com.swd392.group7.phygen.MatrixDetail;

import com.swd392.group7.phygen.MatrixDetail.Enum.DifficultyLevel;
import com.swd392.group7.phygen.MatrixDetail.Enum.Type;
import com.swd392.group7.phygen.MatrixSection.MatrixSection;
import com.swd392.group7.phygen.Topic.Entity.Topic;
import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class MatrixDetail {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    Long id;

    @Enumerated(EnumType.STRING)
    DifficultyLevel difficultyLevel;

    int quantity;

    @Enumerated(EnumType.STRING)
    Type type;

    @ManyToOne
    @JoinColumn(name = "matrix_section_id")
    MatrixSection matrixSection;

    @ManyToOne
    @JoinColumn(name = "topic_id")
    Topic topic;


}
