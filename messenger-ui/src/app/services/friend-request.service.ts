import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import FriendRequest from '../models/FriendRequest';
import AddFriendRequest from '../requests/AddFriendRequest';
import GetFriendRequestsRequest from '../requests/GetFriendRequestsRequest';
import RespondToFriendRequestRequest from '../requests/RespondToFriendRequestRequest';
import { AuthService } from './auth.service';
import ServiceBase from './service-base';
import { HubConnection } from '@microsoft/signalr/dist/esm/HubConnection';

const API_URL = 'https://localhost:5001/api/friend-request'

@Injectable({
  providedIn: 'root'
})
export class FriendRequestService extends ServiceBase {

  wsConnection: HubConnection | null = null

  constructor(private http: HttpClient, authService: AuthService) {
    super(authService)
  }

  postFriendRequest(request: AddFriendRequest): Observable<any> {
    return this.http.post(API_URL, request, { headers: this.httpHeaders() })
  }

  getFriendRequests(request: GetFriendRequestsRequest): Observable<FriendRequest[]> {
    let params = new HttpParams()
    if (request.receiverId) {
      params = params.append('ReceiverId', request.receiverId)
    } else if (request.senderId) {
      params = params.append('SenderId', request.senderId)
    }
    if(request.id){
      params = params.append('Id', request.id)
    }

    return this.http.get<FriendRequest[]>(API_URL, { params, headers: this.httpHeaders() })
  }

  getFriendRequestCount(request: GetFriendRequestsRequest): Observable<number> {
    let params = new HttpParams()
    if (request.receiverId) {
      params = params.append('ReceiverId', request.receiverId)
    } else if (request.senderId) {
      params = params.append('SenderId', request.senderId)
    }

    return this.http.get<number>(`${API_URL}/count`, { params, headers: this.httpHeaders() })
  }

  respondToFriendRequest(request: RespondToFriendRequestRequest): Observable<any> {
    return this.http.put(`${API_URL}/respond`, request, { headers: this.httpHeaders() })
  }

  delete(id: number) : Observable<any> {
    return this.http.delete(`${API_URL}/${id}`, { headers: this.httpHeaders() })
  }
}
