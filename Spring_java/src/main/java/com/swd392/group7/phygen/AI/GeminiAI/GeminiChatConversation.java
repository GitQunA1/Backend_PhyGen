package com.swd392.group7.phygen.AI.GeminiAI;

import lombok.Data;
import org.bson.types.ObjectId;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import java.time.LocalDateTime;
import java.util.List;

@Data
@Document(collection = "Chat")
public class GeminiChatConversation {
    @Id
    ObjectId id;

    String userName;

    List<ConversationResponse> conversationResponses;

    LocalDateTime createdAt;
}
