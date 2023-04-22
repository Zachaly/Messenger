import { Injectable } from '@angular/core';
import ServiceBase from './service-base';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import AddChatMessageReactionRequest from '../requests/AddChatMessageReactionRequest';
import { Observable } from 'rxjs';

const API_URL = 'https://localhost:5001/api/chat-message-reaction'

@Injectable({
  providedIn: 'root'
})
export class ChatMessageReactionService extends ServiceBase {

  constructor(authService: AuthService, private http: HttpClient) {
    super(authService)
  }

  postReaction(request: AddChatMessageReactionRequest): Observable<any> {
    return this.http.post(API_URL, request, {
      headers: this.httpHeaders()
    })
  }

  deleteReaction(userId: number, messageId: number, chatId: number): Observable<any> {
    return this.http.delete(`${API_URL}/${userId}/${messageId}/${chatId}`, {
      headers: this.authorizeHeader()
    })
  }
}
