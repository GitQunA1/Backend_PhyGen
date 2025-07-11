package com.swd392.group7.phygen.Subject;

import com.swd392.group7.phygen.Subject.Entity.Subject;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface SubjectRepo extends JpaRepository<Subject,Long> {
}
