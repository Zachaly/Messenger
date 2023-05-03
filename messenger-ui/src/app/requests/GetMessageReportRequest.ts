import PagedRequest from "./PagedRequest";

export default interface GetMessageReportRequest extends PagedRequest {
    id?: number,
    reportingUserId?: number,
    resolved?: boolean,
    reportedUserId?: boolean,
    attachedMessageId?: number,
    reason?: string,
    reportDate?: Date
}