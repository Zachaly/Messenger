import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import ServiceBase from './service-base';
import { AuthService } from './auth.service';
import ChangeDirectMessageReactionRequest from '../requests/ChangeDirectMessageReactionRequest';
import { Observable } from 'rxjs';

const API_URL = 'https://localhost:5001/api/direct-message-reaction'

@Injectable({
  providedIn: 'root'
})
export class ReactionService extends ServiceBase {

  constructor(private http: HttpClient, authService: AuthService) {
    super(authService);
  }

  changeReaction(request: ChangeDirectMessageReactionRequest): Observable<any> {
    return this.http.put(API_URL, request, { headers: this.httpHeaders() })
  }

  deleteReaction(messageId: number, receiverId: number): Observable<any> {
    return this.http.delete(`${API_URL}/${messageId}/${receiverId}`, { headers: this.authorizeHeader() })
  }
}
