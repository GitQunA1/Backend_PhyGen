package com.swd392.group7.phygen.Question.Entity;

import com.swd392.group7.phygen.Answer.Entity.Answer;
import com.swd392.group7.phygen.ExamQuestion.Entity.ExamQuestion;
import com.swd392.group7.phygen.Question.Enum.DifficultyLevel;
import com.swd392.group7.phygen.Question.Enum.Status;
import com.swd392.group7.phygen.Question.Enum.Type;
import com.swd392.group7.phygen.Topic.Entity.Topic;
import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.util.List;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Question {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    Long id;

    @Column(length = 4000)
    String content;

    @Enumerated(EnumType.STRING)
    Type type;

    @Enumerated(EnumType.STRING)
    DifficultyLevel difficultyLevel;

    @Enumerated(EnumType.STRING)
    Status status;

    @OneToMany(mappedBy = "question",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<ExamQuestion> examQuestions;

    @OneToMany(mappedBy = "question",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<Answer> answers;

    @ManyToOne
    @JoinColumn(name = "topic_id")
    Topic topic;

}
