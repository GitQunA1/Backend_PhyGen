package com.swd392.group7.phygen.AI.GeminiAI;

import org.bson.types.ObjectId;
import org.springframework.data.mongodb.repository.MongoRepository;

import java.util.List;

public interface GeminiRepository extends MongoRepository<GeminiChatConversation, ObjectId>{
    List<GeminiChatConversation> findAllByUserName(String userName);

    GeminiChatConversation findFirstByUserNameOrderByCreatedAtDesc(String userName);
}
