import { HttpParams } from "@angular/common/http";
import GetChatMessageRequest from "../GetChatMessageRequest";
import MapPagedRequest from "./MapPagedRequest";

export default (request: GetChatMessageRequest) : HttpParams => {
    let params = MapPagedRequest(request)

    if(request.id){
        params = params.append('Id', request.id)
    }

    if(request.chatId){
        params = params.append('ChatId', request.chatId)
    }

    return params
}