import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import DirectMessage from '../models/DirectMessage';
import AddDirectMessageRequest from '../requests/AddDirectMessageRequest';
import GetDirectMessageRequest from '../requests/GetDirectMessagesRequest';
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
    let params = new HttpParams()

    if (request.id) {
      params = params.append('Id', request.id)
    }
    if (request.user1Id) {
      params = params.append('User1Id', request.user1Id)
    }
    if (request.user2Id) {
      params = params.append('User2Id', request.user2Id)
    }

    return this.http.get<DirectMessage[]>(API_URL, { headers: this.httpHeaders(), params })
  }

  postMessage(request: AddDirectMessageRequest): Observable<any> {
    return this.http.post(API_URL, request, { headers: this.httpHeaders() })
  }

  updateMessage(request: UpdateDirectMessageRequest): Observable<any> {
    return this.http.put(API_URL, request, { headers: this.httpHeaders() })
  }
}
