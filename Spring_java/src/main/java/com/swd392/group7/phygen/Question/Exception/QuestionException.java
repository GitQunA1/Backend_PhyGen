package com.swd392.group7.phygen.Question.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class QuestionException extends BaseException {
    public QuestionException(String message,HttpStatus status) {
        super(message,status);
    }
}
