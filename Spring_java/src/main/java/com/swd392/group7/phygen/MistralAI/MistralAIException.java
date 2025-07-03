package com.swd392.group7.phygen.MistralAI;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class MistralAIException extends BaseException {
    public MistralAIException(String message, HttpStatus status) {
        super(message,status);
    }
}
