import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import UserListItem from '../models/UserListItem';
import { AuthService } from './auth.service';
import UpdateUsernameRequest from '../requests/UpdateUsernameRequest';
import GetUsersRequest from '../requests/GetUsersRequest';
import MapGetUsersRequest from '../requests/paramMappers/MapGetUsersRequest';
import ChangeUserPasswordRequest from '../requests/ChangeUserPasswordRequest';
import ServiceBase from './service-base';

const API_URL = 'https://localhost:5001/api/user'

@Injectable({
  providedIn: 'root'
})
export class UserService extends ServiceBase {

  constructor(private http: HttpClient, authService: AuthService) {
    super(authService)
  }

  getUsers(request: GetUsersRequest): Observable<UserListItem[]> {
    const params = MapGetUsersRequest(request)

    return this.http.get<UserListItem[]>(API_URL, {
      headers: this.httpHeaders(),
      params
    })
  }

  updateUser(request: UpdateUsernameRequest): Observable<any> {
    return this.http.patch(API_URL, request, {
      headers: this.httpHeaders()
    })
  }

  changePassword(request: ChangeUserPasswordRequest): Observable<any> {
    return this.http.patch(`${API_URL}/change-password`, request, { headers: this.httpHeaders() })
  }
}
