package com.swd392.group7.phygen.AI.Cohere;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class CohereException extends BaseException {
    public CohereException(String message, HttpStatus status) {
        super(message,status);
    }
}
