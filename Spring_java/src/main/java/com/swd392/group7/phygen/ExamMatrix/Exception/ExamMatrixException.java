package com.swd392.group7.phygen.ExamMatrix.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class ExamMatrixException extends BaseException {
    public ExamMatrixException(String message, HttpStatus status) {
        super(message,status);
    }
}
