package com.swd392.group7.phygen.Answer.Entity;

import com.swd392.group7.phygen.Question.Entity.Question;
import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
public class Answer {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    Long id;

    String content;

    boolean isCorrect;

    boolean isDeleted = false;

    @ManyToOne
    @JoinColumn(name ="question_id")
    Question question;
}

