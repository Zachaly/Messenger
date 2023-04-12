import PagedRequest from "./PagedRequest";

export default interface GetChatUserRequest extends PagedRequest {
    userId?: number,
    chatId?: number
}