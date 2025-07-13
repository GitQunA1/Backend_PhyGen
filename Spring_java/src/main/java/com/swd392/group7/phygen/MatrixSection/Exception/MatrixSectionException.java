package com.swd392.group7.phygen.MatrixSection.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class MatrixSectionException extends BaseException {
    public MatrixSectionException(String message, HttpStatus status) {
        super(message,status);
    }
}
