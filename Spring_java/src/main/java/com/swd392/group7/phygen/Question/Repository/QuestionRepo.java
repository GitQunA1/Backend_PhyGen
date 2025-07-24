package com.swd392.group7.phygen.Question.Repository;

import com.swd392.group7.phygen.Question.Entity.Question;
import com.swd392.group7.phygen.Question.Enum.Status;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface QuestionRepo extends JpaRepository<Question, Long> {
    Page<Question> findAllByTopicId(Long topicId, Pageable pageable);
    Optional<List<Question>> findAllByTopicId(Long topicId);

    Page<Question> findAllByStatus(PageRequest pageRequest, Status status);
}
