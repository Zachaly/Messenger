import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import UserListItem from '../models/UserListItem';
import { AuthService } from './auth.service';
import ServiceBase from './service-base';

const API_URL = 'https://localhost:5001/api/friend'

@Injectable({
  providedIn: 'root'
})
export class FriendService extends ServiceBase {

  constructor(private http: HttpClient, authService: AuthService) {
    super(authService)
  }

  getFriends(userId: number): Observable<UserListItem[]> {
    let params = new HttpParams()
    params = params.append('UserId', userId)

    return this.http.get<UserListItem[]>(API_URL, { params, headers: this.httpHeaders() })
  }

  delete(userId: number, friendId: number) : Observable<any> {
    return this.http.delete(`${API_URL}/${userId}/${friendId}`, { headers: this.httpHeaders() })
  }
}
