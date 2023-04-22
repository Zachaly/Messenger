export default interface AddChatMessageReactionRequest {
    userId: number,
    messageId: number,
    chatId: number,
    reaction: string
}