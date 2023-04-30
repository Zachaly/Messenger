import { Injectable } from '@angular/core';
import ServiceBase from './service-base';
import { AuthService } from './auth.service';
import { HttpClient } from '@angular/common/http';
import UserClaim from '../models/UserClaim';
import { Observable } from 'rxjs';
import MapGetUserClaimsRequest from '../requests/paramMappers/MapGetUserClaimsRequest';
import GetUserClaimRequest from '../requests/GetUserClaimsRequest';
import AddUserClaimRequest from '../requests/AddUserClaimRequest';

const API_URL = 'https://localhost:5001/api/user-claim'

@Injectable({
  providedIn: 'root'
})
export class UserClaimService extends ServiceBase {

  constructor(authService: AuthService, private http: HttpClient) {
    super(authService)
  }

  getUserClaims(request: GetUserClaimRequest): Observable<UserClaim[]> {
    const params = MapGetUserClaimsRequest(request)

    return this.http.get<UserClaim[]>(API_URL, { params, headers: this.authorizeHeader() })
  }

  addUserClaim(request: AddUserClaimRequest): Observable<any> {
    return this.http.post(API_URL, request, { headers: this.httpHeaders() })
  }

  deleteUserClaim(userId: number, claim: string): Observable<any> {
    return this.http.delete(`${API_URL}/${userId}/${claim}`, { headers: this.authorizeHeader() })
  }
}
