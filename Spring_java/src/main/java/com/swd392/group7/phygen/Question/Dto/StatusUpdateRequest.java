package com.swd392.group7.phygen.Question.Dto;

import com.swd392.group7.phygen.Question.Enum.Status;
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
public class StatusUpdateRequest {
    List<Long> id;

    Status status;

}
