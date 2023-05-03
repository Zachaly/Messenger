import { Injectable } from '@angular/core';
import ServiceBase from './service-base';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import GetUserBanRequest from '../requests/GetUserBanRequest';
import { Observable } from 'rxjs';
import UserBan from '../models/UserBan';
import MapGetUserBanRequest from '../requests/paramMappers/MapGetUserBanRequest';
import AddUserBanRequest from '../requests/AddUserBanRequest';

const API_URL = 'https://localhost:5001/api/user-ban'

@Injectable({
  providedIn: 'root'
})
export class UserBanService extends ServiceBase {

  constructor(authService: AuthService, private http: HttpClient) {
    super(authService)
  }

  getUserBan(request: GetUserBanRequest): Observable<UserBan[]> {
    const params = MapGetUserBanRequest(request)

    return this.http.get<UserBan[]>(API_URL, { params, headers: this.authorizeHeader() })
  }

  postUserBan(request: AddUserBanRequest): Observable<any> {
    return this.http.post(API_URL, request, { headers: this.httpHeaders() })
  }

  deleteUserBan(id: number): Observable<any> {
    return this.http.delete(`${API_URL}/${id}`, { headers: this.authorizeHeader() })
  }
}
