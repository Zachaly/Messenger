import { HttpParams } from "@angular/common/http";
import GetMessageReportRequest from "../GetMessageReportRequest";
import MapPagedRequest from "./MapPagedRequest";

export default (request: GetMessageReportRequest): HttpParams => {
    let params = MapPagedRequest(request)

    const { id, reason, reportingUserId, reportedUserId, resolved, reportDate, attachedMessageId } = request
    
    if(id){
        params = params.append('Id', id)
    }

    if(reason){
        params = params.append('Reason', reason)
    }

    if(reportedUserId) {
        params = params.append('ReportedUserId', reportedUserId)
    }

    if(reportingUserId){
        params = params.append('ReportingUserId', reportingUserId)
    }

    if(resolved !== undefined){
        params = params.append('Resolved', resolved)
    }

    if(reportDate){
        params = params.append('ReportDate', reportDate.toISOString())
    }

    if(attachedMessageId){
        params = params.append('AttachedMessageId', attachedMessageId)
    }


    return params
}