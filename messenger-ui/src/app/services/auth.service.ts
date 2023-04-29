import { HttpClient, HttpHeaders } from '@angular/common/http';
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

  currentUser: LoginResponse = { userName: '', userId: 0, authToken: '', claims: [] }
  private userSubject: Subject<LoginResponse> = new Subject()

  constructor(private http: HttpClient) {

  }

  register(request: AddUserRequest): Observable<any> {
    return this.http.post(API_URL, request)
  }

  login(request: LoginRequest) {
    return this.http.post<LoginResponse>(`${API_URL}/login`, request).subscribe(res => {
      this.currentUser = res
      this.userSubject.next(this.currentUser)
    })
  }

  onAuthChange(): Subject<LoginResponse> {
    return this.userSubject
  }

  logout() {
    this.currentUser = { userName: '', userId: 0, authToken: '', claims: [] }
    this.userSubject.next(this.currentUser)
  }

  saveUserData() {
    localStorage.setItem('token', this.currentUser.authToken)
  }

  clearUserData() {
    localStorage.setItem('token', '')
  }

  loadUserData() {
    const token = localStorage.getItem('token')

    if(!token){
      return
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    })

    this.http.get<LoginResponse>(`${API_URL}/current`, { headers }).subscribe(res => {
      this.currentUser = res
      this.userSubject.next(this.currentUser)
    })
  }
}
