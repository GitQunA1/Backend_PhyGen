package com.swd392.group7.phygen.ExamSection.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class ExamSectionException extends BaseException {
    public ExamSectionException(String message, HttpStatus status) {
        super(message,status);
    }
}
