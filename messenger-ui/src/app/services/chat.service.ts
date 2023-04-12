import { Injectable } from '@angular/core';
import ServiceBase from './service-base';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import AddChatRequest from '../requests/AddChatRequest';
import { Observable } from 'rxjs';
import GetChatRequest from '../requests/GetChatRequest';
import ChatModel from '../models/ChatModel';
import MapGetChatRequest from '../requests/paramMappers/MapGetChatRequest';
import UpdateChatRequest from '../requests/UpdateChatRequest';

const API_URL = 'https://localhost:5001/api/chat'

@Injectable({
  providedIn: 'root'
})
export class ChatService extends ServiceBase {

  constructor(authService: AuthService, private http: HttpClient) {
    super(authService)
  }

  addChat(request: AddChatRequest): Observable<any> {
    return this.http.post(API_URL, request, { headers: this.httpHeaders() })
  }

  getChat(request: GetChatRequest): Observable<ChatModel[]> {
    const params = MapGetChatRequest(request)

    return this.http.get<ChatModel[]>(API_URL, { params, headers: this.httpHeaders() })
  }

  updateChat(request: UpdateChatRequest): Observable<any> {
    return this.http.put(API_URL, request, { headers: this.httpHeaders() })
  }
}
