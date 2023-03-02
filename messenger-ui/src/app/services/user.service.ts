import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import UserListItem from '../models/UserListItem';
import PagedRequest from '../requests/PagedRequest';
import { AuthService } from './auth.service';

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

  getUsers(request: PagedRequest): Observable<UserListItem[]> {
    let params = new HttpParams()

    if (request.pageIndex) {
      params = params.append('PageIndex', request.pageIndex)
    }

    if (request.pageSize) {
      params = params.append('PageSize', request.pageSize)
    }
    
    return this.http.get<UserListItem[]>(API_URL, {
      headers: this.httpHeaders(),
      params
    })
  }
}
