package com.swd392.group7.phygen.MatrixDetail.Repository;

import com.swd392.group7.phygen.MatrixDetail.Enum.DifficultyLevel;
import com.swd392.group7.phygen.MatrixDetail.MatrixDetail;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface MatrixDetailRepo extends JpaRepository<MatrixDetail,Long> {
    Optional<List<MatrixDetail>> findAllByMatrixSectionIdAndDifficultyLevel(Long id, DifficultyLevel difficultLevel);
}
