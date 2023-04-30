import { HttpParams } from "@angular/common/http";
import GetUserClaimRequest from "../GetUserClaimsRequest";
import MapPagedRequest from "./MapPagedRequest";

export default (request: GetUserClaimRequest): HttpParams => {
    let params = MapPagedRequest(request)

    if(request.userId){
        params = params.append('UserId', request.userId)
    }

    if(request.value){
        params = params.append('Value', request.value)
    }

    return params
}