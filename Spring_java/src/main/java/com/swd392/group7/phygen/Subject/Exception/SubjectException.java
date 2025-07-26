package com.swd392.group7.phygen.Subject.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class SubjectException extends BaseException {
    public SubjectException(String message, HttpStatus status) {
        super(message,status);
    }
}
