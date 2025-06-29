package com.swd392.group7.phygen.Common;

import com.swd392.group7.phygen.Answer.Exception.AnswerException;
import com.swd392.group7.phygen.Question.Exception.QuestionException;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

import java.util.List;

@RestControllerAdvice
public class GlobalExceptionHandler {

    @ExceptionHandler(BaseException.class)
    public ResponseEntity<ApiResponse<Object>> handleBaseException(BaseException ex) {
        ApiResponse<Object> response = new ApiResponse<>(
                false,
                ex.getMessage(),
                null,
                List.of(ex.getMessage())
        );
        return new ResponseEntity<>(response, ex.getStatus());
    }


}
