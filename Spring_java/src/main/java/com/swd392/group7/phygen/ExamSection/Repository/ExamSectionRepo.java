package com.swd392.group7.phygen.ExamSection.Repository;

import com.swd392.group7.phygen.ExamSection.Entity.ExamSection;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface ExamSectionRepo extends JpaRepository<ExamSection, Long> {
    Page<ExamSection> findAllByExamId(Long examId, Pageable pageable);

    Optional<List<ExamSection>> findAllByExamId(Long examId);
}
