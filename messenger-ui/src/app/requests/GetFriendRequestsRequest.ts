import PagedRequest from "./PagedRequest"

export default interface GetFriendRequestsRequest extends PagedRequest {
    senderId?: number,
    receiverId?: number
    id?: number
}