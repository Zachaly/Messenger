import { HttpParams } from "@angular/common/http";
import GetChatUserRequest from "../GetChatUserRequest";
import MapPagedRequest from "./MapPagedRequest";

export default (request: GetChatUserRequest): HttpParams => {
    let params = MapPagedRequest(request)

    if(request.chatId){
        params = params.append('ChatId', request.chatId)
    }

    if(request.userId){
        params = params.append('UserId', request.userId)
    }

    return params
}