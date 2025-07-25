package com.swd392.group7.phygen.MatrixDetail.Dto;

import com.swd392.group7.phygen.MatrixDetail.Enum.DifficultyLevel;
import com.swd392.group7.phygen.MatrixDetail.Enum.Type;
import com.swd392.group7.phygen.Topic.Dto.TopicStructureResponse;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class MatrixDetailStructureResponse {
    DifficultyLevel difficultyLevel;
    int quantity;
    Type type;
    TopicStructureResponse topicStructureResponse;
}
