import { HttpParams } from "@angular/common/http";
import GetFriendRequestsRequest from "../GetFriendRequestsRequest";
import MapPagedRequest from "./MapPagedRequest";

export default (request: GetFriendRequestsRequest): HttpParams => {

    let params = MapPagedRequest(request)
    
    const { id, senderId, receiverId } = request

    if (id) {
        params = params.append('Id', id)
    }
    if (senderId) {
        params = params.append('SenderId', senderId)
    }
    if (receiverId) {
        params = params.append('ReceiverId', receiverId)
    }

    return params
}