export default interface DirectMessage {
    id: number,
    content: string,
    read: boolean,
    created: string,
    senderName: string,
    senderId: number
}