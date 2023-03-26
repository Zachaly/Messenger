import { HttpParams } from "@angular/common/http"
import PagedRequest from "./PagedRequest"

export interface GetDirectMessageRequest extends PagedRequest {
    user1Id?: number,
    user2Id?: number,
    id?: number,
    senderId?: number,
    receiverId?: number,
    read?: boolean,
}