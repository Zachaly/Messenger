import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import UserListItem from '../models/UserListItem';
import { AuthService } from './auth.service';
import UpdateUsernameRequest from '../requests/UpdateUsernameRequest';
import GetUsersRequest from '../requests/GetUsersRequest';
import MapGetUsersRequest from '../requests/paramMappers/MapGetUsersRequest';

const API_URL = 'https://localhost:5001/api/user'

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private token = () => this.authService.currentUser.authToken

  httpHeaders = () =>
    new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.token()}`
    })


  constructor(private http: HttpClient, private authService: AuthService) {

  }

  getUsers(request: GetUsersRequest): Observable<UserListItem[]> {
    const params = MapGetUsersRequest(request)
    
    return this.http.get<UserListItem[]>(API_URL, {
      headers: this.httpHeaders(),
      params
    })
  }

  updateUser(request: UpdateUsernameRequest) : Observable<any> {
    return this.http.patch(API_URL, request, {
      headers: this.httpHeaders()
    })
  }
}
