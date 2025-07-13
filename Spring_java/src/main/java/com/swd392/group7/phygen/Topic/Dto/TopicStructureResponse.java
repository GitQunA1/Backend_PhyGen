package com.swd392.group7.phygen.Topic.Dto;

import com.swd392.group7.phygen.Question.Dto.QuestionStructureResponse;
import com.swd392.group7.phygen.Topic.Enum.Level;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

import java.util.List;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class TopicStructureResponse {
    String name;
    Level level;
    List<QuestionStructureResponse> questionStructureResponses;
}
