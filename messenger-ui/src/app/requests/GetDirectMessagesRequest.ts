import { HttpParams } from "@angular/common/http"

export interface GetDirectMessageRequest {
    user1Id?: number,
    user2Id?: number,
    id?: number,
    senderId?: number,
    receiverId?: number,
    read?: boolean,
    pageIndex?: number,
    pageSize?: number
}

export const mapGetDirectMessageRequestToParams = (request: GetDirectMessageRequest): HttpParams => {
    let params = new HttpParams()
    if (request.id) {
        params = params.append('Id', request.id)
    }
    if (request.user1Id) {
        params = params.append('User1Id', request.user1Id)
    }
    if (request.user2Id) {
        params = params.append('User2Id', request.user2Id)
    }
    if (request.read !== undefined && request.read !== null) {
        params = params.append('Read', request.read)
    }
    if (request.senderId) {
        params = params.append('SenderId', request.senderId)
    }
    if (request.receiverId) {
        params = params.append('ReceiverId', request.receiverId)
    }
    if(request.pageIndex){
        params = params.append('PageIndex', request.pageIndex)
    }
    if(request.pageSize) {
        params = params.append('PageSize', request.pageSize)
    }

    return params
}