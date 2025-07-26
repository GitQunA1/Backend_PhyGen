package com.swd392.group7.phygen.AI.Structure.Dto;

import com.swd392.group7.phygen.Question.Enum.DifficultyLevel;
import com.swd392.group7.phygen.Question.Enum.Type;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
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
public class QuestionDto {

    String content;

    @Enumerated(EnumType.STRING)
    Type type;

    @Enumerated(EnumType.STRING)
    DifficultyLevel difficultyLevel;

    List<AnswerDto> answers;
}
