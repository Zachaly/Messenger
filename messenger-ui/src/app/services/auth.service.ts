import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import LoginResponse from '../models/LoginResponse';
import AddUserRequest from '../requests/AddUserRequest';
import LoginRequest from '../requests/LoginRequest';

const API_URL = 'https://localhost:5001/api/user'

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  currentUser: LoginResponse = { userName: '', userId: 0, authToken: '' }
  private userSubject: Subject<LoginResponse> = new Subject()

  constructor(private http: HttpClient) {

  }

  register(request: AddUserRequest): Observable<any> {
    return this.http.post(API_URL, request)
  }

  login(request: LoginRequest, onLogin: Function) {
    return this.http.post<LoginResponse>(`${API_URL}/login`, request).subscribe(res => {
      this.currentUser = res
      this.userSubject.next(this.currentUser)
      onLogin()
    })
  }

  onAuthChange(): Subject<LoginResponse> {
    return this.userSubject
  }

  logout() {
    this.currentUser = { userName: '', userId: 0, authToken: '' }
    this.userSubject.next(this.currentUser)
  }
}
