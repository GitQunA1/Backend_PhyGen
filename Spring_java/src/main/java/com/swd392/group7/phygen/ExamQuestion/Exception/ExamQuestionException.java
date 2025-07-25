package com.swd392.group7.phygen.ExamQuestion.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class ExamQuestionException extends BaseException {
    public ExamQuestionException(String message, HttpStatus status) {
        super(message,status);
    }
}
