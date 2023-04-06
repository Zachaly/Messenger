import { HttpParams } from "@angular/common/http";
import GetUsersRequest from "../GetUsersRequest";
import MapPagedRequest from "./MapPagedRequest";

export default (request: GetUsersRequest) : HttpParams => {
    let params = MapPagedRequest(request)

    if(request.id){
        params = params.append('Id', request.id)
    }

    return params
}