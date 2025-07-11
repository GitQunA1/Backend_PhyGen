package com.swd392.group7.phygen.AI.Structure.Dto;

import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.FieldDefaults;

import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class MistralAPIResponse<T> {
    boolean success;
    String message;
    String downloadLink;
    T data;
    List<String> errors;
}
