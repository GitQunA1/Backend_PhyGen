package com.swd392.group7.phygen.Answer.Repository;

import com.swd392.group7.phygen.Answer.Entity.Answer;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface AnswerRepo extends JpaRepository<Answer, Long> {

    Page<Answer> findAllByQuestionId (Long questionId, Pageable pageable);
}
