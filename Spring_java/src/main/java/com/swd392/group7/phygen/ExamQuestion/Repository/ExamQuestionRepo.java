package com.swd392.group7.phygen.ExamQuestion.Repository;

import com.swd392.group7.phygen.ExamQuestion.Entity.ExamQuestion;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface ExamQuestionRepo extends JpaRepository<ExamQuestion,Long> {
    Page<ExamQuestion> findAllByQuestionId(Long questionId, Pageable pageable);

    Page<ExamQuestion> findAllByExamSectionId(Long examSectionId, Pageable pageable);
}
