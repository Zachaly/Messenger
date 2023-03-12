import { Component, OnInit } from '@angular/core';
import FriendRequestResponse from 'src/app/models/FriendRequestResponse';
import { AuthService } from 'src/app/services/auth.service';
import { FriendRequestService } from 'src/app/services/friend-request.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css']
})
export class NotificationListComponent {

  notifications: string[] = []

  constructor(private authService: AuthService, private signalRService: SignalrService, friendRequestService: FriendRequestService) {
    this.authService.onAuthChange().subscribe(res => {
      if(!res.authToken){
        this.signalRService.friendConnection?.stop()
        return
      }

      this.signalRService.openFriendConnection()
      this.signalRService.setFriendConnectionListener('GetRequest', (id: number) => {
        friendRequestService.getFriendRequests({ receiverId: this.authService.currentUser.userId })
        .subscribe(x => {
          const text = `${x.find(req => req.id == id)!.name} send you a friend request`
          this.notifications.push(text)
        })
      })
      this.signalRService.setFriendConnectionListener('GetRequestResponse', (x: FriendRequestResponse) => {
        const text = `${x.name} ${x.accepted ? 'accepted': 'denied'} your request`
        this.notifications.push(text)
      })
    })

  }

  remove(index: number) {
    this.notifications.splice(index, 1)
  }
}
