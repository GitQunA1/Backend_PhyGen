package com.swd392.group7.phygen.AI.Structure.Dto;

import com.swd392.group7.phygen.Topic.Enum.Level;
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
public class TopicDto {

    String name;

    @Enumerated(EnumType.STRING)
    Level level;

    List<TopicDto> subTopics;

    List<QuestionDto> questions;
}
