package com.swd392.group7.phygen.Exam.Repository;

import com.swd392.group7.phygen.Exam.Entity.Exam;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface ExamRepo extends JpaRepository<Exam, Long> {
    Page<Exam> findAllByAccountId(Long accountId, Pageable pageable);
}
