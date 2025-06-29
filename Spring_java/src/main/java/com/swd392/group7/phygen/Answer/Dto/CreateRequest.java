package com.swd392.group7.phygen.Answer.Dto;

import com.swd392.group7.phygen.Question.Entity.Question;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class CreateRequest {

    String content;

    boolean isCorrect;
}
