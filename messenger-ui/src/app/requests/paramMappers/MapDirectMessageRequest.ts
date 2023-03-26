import { HttpParams } from "@angular/common/http";
import { GetDirectMessageRequest } from "../GetDirectMessagesRequest";
import MapPagedRequest from "./MapPagedRequest";

export default (request: GetDirectMessageRequest): HttpParams => {

    let params = MapPagedRequest(request)

    const { id, user1Id, user2Id, read, senderId, receiverId } = request

    if (id) {
        params = params.append('Id', id)
    }
    if (user1Id) {
        params = params.append('User1Id', user1Id)
    }
    if (user2Id) {
        params = params.append('User2Id', user2Id)
    }
    if (read !== undefined && read !== null) {
        params = params.append('Read', read)
    }
    if (senderId) {
        params = params.append('SenderId', senderId)
    }
    if (receiverId) {
        params = params.append('ReceiverId', receiverId)
    }

    return params
}