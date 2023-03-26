import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import DirectMessage from '../models/DirectMessage';
import AddDirectMessageRequest from '../requests/AddDirectMessageRequest';
import { GetDirectMessageRequest, mapGetDirectMessageRequestToParams } from '../requests/GetDirectMessagesRequest';
import UpdateDirectMessageRequest from '../requests/UpdateDirectMessageRequest';
import { AuthService } from './auth.service';
import ServiceBase from './service-base';

const API_URL = 'https://localhost:5001/api/direct-message'

@Injectable({
  providedIn: 'root'
})
export class DirectMessageService extends ServiceBase {

  constructor(authService: AuthService, private http: HttpClient) {
    super(authService)
  }

  getMessages(request: GetDirectMessageRequest): Observable<DirectMessage[]> {
    const params = mapGetDirectMessageRequestToParams(request)

    return this.http.get<DirectMessage[]>(API_URL, { headers: this.httpHeaders(), params })
  }

  postMessage(request: AddDirectMessageRequest): Observable<any> {
    return this.http.post(API_URL, request, { headers: this.httpHeaders() })
  }

  updateMessage(request: UpdateDirectMessageRequest): Observable<any> {
    return this.http.put(API_URL, request, { headers: this.httpHeaders() })
  }

  getMessageCount(request: GetDirectMessageRequest): Observable<number> {
    const params = mapGetDirectMessageRequestToParams(request)
    return this.http.get<number>(`${API_URL}/count`, { headers: this.httpHeaders(), params })
  }
}
