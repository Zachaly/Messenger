import { HttpParams } from "@angular/common/http";
import GetUserBanRequest from "../GetUserBanRequest";
import MapPagedRequest from "./MapPagedRequest";

export default (request: GetUserBanRequest): HttpParams => {
    let params = MapPagedRequest(request)

    const { id, userId, start, end } = request

    if (id) {
        params = params.append('Id', id)
    }

    if (userId) {
        params = params.append('UserId', userId)
    }

    if (start) {
        params = params.append('Start', start.toISOString())
    }

    if (end) {
        params = params.append('End', end.toISOString())
    }

    return params
}