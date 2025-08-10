export interface CreateConversationRequest {
    conversationName: string | null;
    userIds: number[];
}

export interface ConversationDto {
    conversationId: number;
    conversationName: string | null;
    isOneToOne: boolean;
}