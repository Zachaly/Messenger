import { Injectable } from '@angular/core';
import ServiceBase from './service-base';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import AddChatUserRequest from '../requests/AddChatUserRequest';
import { Observable } from 'rxjs';
import GetChatUserRequest from '../requests/GetChatUserRequest';
import ChatUserModel from '../models/ChatUserModel';
import MapGetChatUserRequest from '../requests/paramMappers/MapGetChatUserRequest';
import UpdateChatUserRequest from '../requests/UpdateChatUserRequest';

const API_URL = 'https://localhost:5001/api/chat-user'

@Injectable({
  providedIn: 'root'
})
export class ChatUserService extends ServiceBase {

  constructor(authService: AuthService, private http: HttpClient) {
    super(authService)
  }

  addChatUser(request: AddChatUserRequest): Observable<any> {
    return this.http.post(API_URL, request, { headers: this.httpHeaders() })
  }

  getChatUser(request: GetChatUserRequest): Observable<ChatUserModel[]> {
    const params = MapGetChatUserRequest(request)

    return this.http.get<ChatUserModel[]>(API_URL, { params, headers: this.authorizeHeader() })
  }

  getChatUserCount(request: GetChatUserRequest): Observable<number> {
    const params = MapGetChatUserRequest(request)

    return this.http.get<number>(`${API_URL}/count`, { params, headers: this.authorizeHeader() })
  }

  updateChatUserRequest(request: UpdateChatUserRequest): Observable<any> {
    return this.http.put(API_URL, request, { headers: this.httpHeaders() })
  }

  deleteChatUser(chatId: number, userId: number): Observable<any> {
    return this.http.delete(`${API_URL}/${chatId}/${userId}`, { headers: this.authorizeHeader() })
  }
}
