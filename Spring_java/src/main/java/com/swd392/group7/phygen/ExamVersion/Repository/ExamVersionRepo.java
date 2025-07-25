package com.swd392.group7.phygen.ExamVersion.Repository;

import com.swd392.group7.phygen.ExamVersion.Entity.ExamVersion;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface ExamVersionRepo extends JpaRepository<ExamVersion,Long> {

    Page<ExamVersion> findAllByExamId(Long examId, Pageable pageable);
}
