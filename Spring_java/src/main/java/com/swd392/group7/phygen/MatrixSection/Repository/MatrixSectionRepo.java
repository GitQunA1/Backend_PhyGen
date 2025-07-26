package com.swd392.group7.phygen.MatrixSection.Repository;

import com.swd392.group7.phygen.MatrixSection.MatrixSection;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface MatrixSectionRepo extends JpaRepository<MatrixSection, Long> {
    Optional<List<MatrixSection>> findAllByExamMatrixId(Long id);
}
