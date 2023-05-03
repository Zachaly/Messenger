import { Injectable } from '@angular/core';
import ServiceBase from './service-base';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import GetMessageReportRequest from '../requests/GetMessageReportRequest';
import { Observable } from 'rxjs';
import MessageReport from '../models/MessageReport';
import MapGetMessageReportRequest from '../requests/paramMappers/MapGetMessageReportRequest';
import AddMessageReportRequest from '../requests/AddMessageReportRequest';
import UpdateMessageReportRequest from '../requests/UpdateMessageReportRequest';

const API_URL = 'https://localhost:5001/api/message-report'

@Injectable({
  providedIn: 'root'
})
export class MessageReportService extends ServiceBase {

  constructor(authService: AuthService, private http: HttpClient) {
    super(authService)
  }

  getMessageReport(request: GetMessageReportRequest): Observable<MessageReport[]> {
    const params = MapGetMessageReportRequest(request)

    return this.http.get<MessageReport[]>(API_URL, { params, headers: this.authorizeHeader() })
  }

  postMessageReport(request: AddMessageReportRequest): Observable<any> {
    return this.http.post(API_URL, request, { headers: this.httpHeaders() })
  }

  updateMessageReport(request: UpdateMessageReportRequest): Observable<any> {
    return this.http.put(API_URL, request, { headers: this.httpHeaders() })
  }
}
