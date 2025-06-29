package com.swd392.group7.phygen.Subject.Converter;

import com.swd392.group7.phygen.Subject.Enum.Grade;
import jakarta.persistence.AttributeConverter;
import jakarta.persistence.Converter;

@Converter(autoApply = true)
public class GradeConverter implements AttributeConverter<Grade, String> {

    @Override
    public String convertToDatabaseColumn(Grade grade) {
        return grade != null ? grade.getValue() : null;
    }

    @Override
    public Grade convertToEntityAttribute(String s) {
        return s != null ? Grade.fromValue(s) : null;
    }
}
