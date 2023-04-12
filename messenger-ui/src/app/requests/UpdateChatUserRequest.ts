export default interface UpdateChatUserRequest {
    userId: number,
    chatId: number,
    isAdmin?: boolean
}