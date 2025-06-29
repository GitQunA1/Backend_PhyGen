package com.swd392.group7.phygen.Topic.Repository;

import com.swd392.group7.phygen.Topic.Entity.Topic;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface TopicRepo extends JpaRepository<Topic, Long> {
}
