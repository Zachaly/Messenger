import { Injectable } from '@angular/core';
import ServiceBase from './service-base';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import AddChatMessageRequest from '../requests/AddChatMessageRequest';
import { Observable } from 'rxjs';
import GetChatMessageRequest from '../requests/GetChatMessageRequest';
import MapChatMessageRequest from '../requests/paramMappers/MapChatMessageRequest';
import ChatMessageModel from '../models/ChatMessageModel';

const API_URL = 'https://localhost:5001/api/chat-message'

@Injectable({
  providedIn: 'root'
})
export class ChatMessageService extends ServiceBase {

  constructor(authService: AuthService, private http: HttpClient) {
    super(authService)
  }

  addChatMessage(request: AddChatMessageRequest): Observable<any> {
    return this.http.post(API_URL, request, { headers: this.httpHeaders() })
  }

  getChatMessage(request: GetChatMessageRequest) : Observable<ChatMessageModel[]> {
    const params = MapChatMessageRequest(request)

    return this.http.get<ChatMessageModel[]>(API_URL, { params, headers: this.authorizeHeader() })
  }

  getChatMessageCount(request: GetChatMessageRequest) : Observable<number> {
    const params = MapChatMessageRequest(request)

    return this.http.get<number>(`${API_URL}/count`, { params, headers: this.authorizeHeader() })
  }
}
