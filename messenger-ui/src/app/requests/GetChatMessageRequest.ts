import PagedRequest from "./PagedRequest";

export default interface GetChatMessageRequest extends PagedRequest {
    id?: number,
    chatId?: number
}