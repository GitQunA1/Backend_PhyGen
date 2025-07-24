package com.swd392.group7.phygen.Subject.Repository;

import com.swd392.group7.phygen.Subject.Entity.Subject;
import com.swd392.group7.phygen.Subject.Enum.Grade;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface SubjectRepo extends JpaRepository<Subject,Long> {
    Optional<Subject> findByNameContainingIgnoreCaseAndGrade(String name, Grade grade);
}
