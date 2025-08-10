export interface GetMessagesResponseDto {
    messageId: number;
    messageContent: string;
    userId: number;
    conversationId: number;
}

export interface MessageDto {
    messageId: number;
    messageContent: string;
    userId: number;
    conversationId: number;
    isMe : boolean;
}