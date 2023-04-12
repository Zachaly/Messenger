import PagedRequest from "./PagedRequest";

export default interface GetChatRequest extends PagedRequest {
    id?: number,
    userId?: number
}