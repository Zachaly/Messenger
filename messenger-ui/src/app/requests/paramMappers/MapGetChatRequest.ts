import { HttpParams } from "@angular/common/http";
import GetChatRequest from "../GetChatRequest";
import MapPagedRequest from "./MapPagedRequest";

export default (request: GetChatRequest): HttpParams => {
    let params = MapPagedRequest(request)

    if (request.id) {
        params = params.append('Id', request.id)
    }

    if (request.userId) {
        params = params.append('UserId', request.userId)
    }

    return params
}