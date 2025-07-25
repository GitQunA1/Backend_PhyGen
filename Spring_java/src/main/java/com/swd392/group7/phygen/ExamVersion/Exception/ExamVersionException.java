package com.swd392.group7.phygen.ExamVersion.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class ExamVersionException extends BaseException {
    public ExamVersionException(String message, HttpStatus status) {
        super(message,status);
    }
}
