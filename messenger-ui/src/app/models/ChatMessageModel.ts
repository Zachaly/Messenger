import ChatMessageReaction from "./ChatMessageReaction";

export default interface ChatMessageModel {
    id: number,
    senderId: number,
    senderName: string,
    readByIds: number[],
    content: string,
    imageIds: number[],
    reactions: ChatMessageReaction[]
}