import MessageType from "./enum/MessageType";

export default interface MessageReport {
    id: number,
    reportingUserId: number,
    reportingUserName: string,
    resolved: boolean,
    reportedUserId: number,
    attachedMessageId: number,
    reason: string,
    reportDate: string,
    messageType: MessageType
}