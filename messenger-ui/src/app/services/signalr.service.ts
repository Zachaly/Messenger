import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr/dist/esm/HubConnectionBuilder';
import { IHttpConnectionOptions } from '@microsoft/signalr/dist/esm/IHttpConnectionOptions';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  friendConnection: HubConnection | null = null

  constructor(private authService: AuthService) { }

  openFriendConnection() {
    const options: IHttpConnectionOptions = {
      headers: { 'Authorization': 'Bearer ' + this.authService.currentUser.authToken },
      withCredentials: true,
      accessTokenFactory: () => this.authService.currentUser.authToken
    }

    this.friendConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:5001/ws/friend', options)
      .build()

    this.friendConnection?.start()
  }

  setFriendConnectionListener(event: string, callback: (...args: any[]) => any) {
    this.friendConnection?.on(event, callback)
  }
}
