package com.swd392.group7.phygen.Common;

import lombok.Getter;
import org.springframework.http.HttpStatus;

@Getter
public class BaseException extends RuntimeException {
    private final HttpStatus status;
    public BaseException(String message,HttpStatus status) {
        super(message);
        this.status = status;
    }
}
