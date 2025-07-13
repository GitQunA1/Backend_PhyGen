package com.swd392.group7.phygen.Question.Dto;

import com.swd392.group7.phygen.Answer.Dto.AnswerStructureResponse;
import com.swd392.group7.phygen.Question.Enum.DifficultyLevel;
import com.swd392.group7.phygen.Question.Enum.Type;
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
public class QuestionStructureResponse {
    String content;
    Type type;
    DifficultyLevel difficultyLevel;
    List<AnswerStructureResponse> answerStructureResponses;
}
