package com.swd392.group7.phygen.Topic.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class TopicException extends BaseException {
    public TopicException(String message, HttpStatus status) {
        super(message,status);
    }
}
