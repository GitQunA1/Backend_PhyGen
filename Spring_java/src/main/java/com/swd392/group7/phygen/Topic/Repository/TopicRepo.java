package com.swd392.group7.phygen.Topic.Repository;

import com.swd392.group7.phygen.Topic.Entity.Topic;
import com.swd392.group7.phygen.Topic.Enum.Level;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface TopicRepo extends JpaRepository<Topic, Long> {
    Optional<Topic> findTopicByIdAndLevel(Long id, Level level);
}
