package com.swd392.group7.phygen.ExamVersion.Dto;

import lombok.*;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ExamVersionUpdateRequest {
    String versionCode;

    String pdfUrl;
}
