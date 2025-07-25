package com.swd392.group7.phygen.ExamMatrix.Repository;

import com.swd392.group7.phygen.ExamMatrix.Enum.ExamType;
import com.swd392.group7.phygen.ExamMatrix.Entity.ExamMatrix;
import com.swd392.group7.phygen.ExamMatrix.Enum.ExamMatrixStatus;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface ExamMatrixRepo extends JpaRepository<ExamMatrix,Long> {

    Optional<ExamMatrix> findBySubjectIdAndExamTypeAndExamMatrixStatus(Long subjectId, ExamType examType, ExamMatrixStatus examMatrixStatus);

    Optional<List<ExamMatrix>> findAllBySubjectId(Long subjectId);
}
