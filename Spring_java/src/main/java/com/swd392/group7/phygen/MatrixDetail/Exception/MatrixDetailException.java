package com.swd392.group7.phygen.MatrixDetail.Exception;

import com.swd392.group7.phygen.Common.BaseException;
import org.springframework.http.HttpStatus;

public class MatrixDetailException extends BaseException {
  public MatrixDetailException(String message, HttpStatus status) {
    super(message,status);
  }
}
