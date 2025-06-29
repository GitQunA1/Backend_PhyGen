package com.swd392.group7.phygen.Exam.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class ExamException extends BaseException {
    public ExamException(String message,HttpStatus status)
    {
        super(message,status);
    }
}
