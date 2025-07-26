package com.swd392.group7.phygen.Answer.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class AnswerException extends BaseException {
    public AnswerException(String message, HttpStatus status) {
        super(message,status);
    }
}
