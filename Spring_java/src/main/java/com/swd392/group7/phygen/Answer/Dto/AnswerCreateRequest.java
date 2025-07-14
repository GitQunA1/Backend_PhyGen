package com.swd392.group7.phygen.Answer.Dto;

import lombok.*;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class AnswerCreateRequest {

    String content;

    boolean isCorrect;
}
