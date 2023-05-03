import MessageType from "src/app/models/enum/MessageType";

export default interface AddMessageReportRequest {
    messageId: number,
    userId: number,
    reason: string,
    reportedUserId: number,
    messageType: MessageType
}