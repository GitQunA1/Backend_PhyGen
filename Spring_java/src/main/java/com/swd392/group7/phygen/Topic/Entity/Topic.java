package com.swd392.group7.phygen.Topic.Entity;

import com.swd392.group7.phygen.Question.Entity.Question;
import com.swd392.group7.phygen.Topic.Enum.Level;
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
public class Topic {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    Long id;
    String name;
    @Enumerated(EnumType.STRING)
    Level level;

    @OneToMany(mappedBy = "topic",cascade = CascadeType.ALL, fetch = FetchType.LAZY)
    List<Question> questions;

    @ManyToOne
    @JoinColumn(name = "topic_id")
    Topic topic;

}
