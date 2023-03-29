import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr/dist/esm/HubConnectionBuilder';
import { IHttpConnectionOptions } from '@microsoft/signalr/dist/esm/IHttpConnectionOptions';
import { AuthService } from './auth.service';

const WS_ENDPOINT = 'https://localhost:5001/ws'

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private connections: HubConnection[] = []

  friendConnection: HubConnection | null = null
  directMessageConnection: HubConnection | null = null

  constructor(private authService: AuthService) { }

  async openConnection(name: string) : Promise<string> {
    const options: IHttpConnectionOptions = {
      headers: { 'Authorization': 'Bearer ' + this.authService.currentUser.authToken },
      withCredentials: true,
      accessTokenFactory: () => this.authService.currentUser.authToken
    }

    const connection = new HubConnectionBuilder()
      .withUrl(`${WS_ENDPOINT}/${name}`, options)
      .build()
    
    await connection.start()

    this.connections.push(connection)

    return connection.connectionId!
  }

  setConnectionListener(id: string, event: string, callback: (...args: any[]) => any){
    const connection = this.connections.find(x => x.connectionId == id)

    connection?.on(event, callback)
  }

  stopAllConnections() {
    this.connections.forEach(conn => conn.stop())
    this.connections = []
  }

  invoke<T>(id: string, method: string) : Promise<T> {
    const connection = this.connections.find(x => x.connectionId == id)

    return connection?.invoke<T>(method)!
  }

  invokeWithArgs(id: string, method: string, ...args: any[]){
    const connection = this.connections.find(x => x.connectionId == id)

    connection?.invoke(method, args)
  }
}
