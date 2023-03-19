export default interface DirectMessage {
    id: number,
    content: string,
    read: boolean,
    created: Date,
    senderName: string,
    senderId: number
}