import { Injectable } from '@angular/core';
import ServiceBase from './service-base';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import AddChatMessageReadRequest from '../requests/AddChatMessageReadRequest';
import { Observable } from 'rxjs';

const API_URL = 'https://localhost:5001/api/chat-message-read'

@Injectable({
  providedIn: 'root'
})
export class ChatMessageReadService extends ServiceBase {

  constructor(authService: AuthService, private http: HttpClient) {
    super(authService)
  }

  addChatMessageRead(request: AddChatMessageReadRequest) : Observable<any> {
    return this.http.post(API_URL, request, { headers: this.httpHeaders() })
  }
}
