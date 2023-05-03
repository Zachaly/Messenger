export default interface UpdateMessageReportRequest {
    id: number,
    resolved?: boolean,
    attachedMessageId?: number,
    reason?: string
}