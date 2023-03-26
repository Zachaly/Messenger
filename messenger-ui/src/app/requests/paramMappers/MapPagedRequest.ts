import { HttpParams } from "@angular/common/http";
import PagedRequest from "../PagedRequest";

export default (request: PagedRequest): HttpParams => {

    let params = new HttpParams()
    
    const { pageIndex, pageSize } = request

    if(pageIndex) {
        params = params.append('PageIndex', pageIndex)
    }
    if(pageSize){
        params = params.append('PageSize', pageSize)
    }

    return params
}