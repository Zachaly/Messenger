import { Component } from '@angular/core';
import DirectMessage from 'src/app/models/DirectMessage';
import FriendRequestResponse from 'src/app/models/FriendRequestResponse';
import { AuthService } from 'src/app/services/auth.service';
import { FriendRequestService } from 'src/app/services/friend-request.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css'],
  providers: [SignalrService]
})
export class NotificationListComponent {

  notifications: string[] = []

  constructor(private authService: AuthService, private signalR: SignalrService, private friendRequestService: FriendRequestService) {
    this.authService.onAuthChange().subscribe(res => {
      if (!res.authToken) {
        this.signalR.stopAllConnections()
        return
      }

      this.setListeners()
    })
  }

  async setListeners() {
    const friendConnectionId = await this.signalR.openConnection('friend')

    this.signalR.setConnectionListener(friendConnectionId, 'GetRequest', (id: number) => {
      this.friendRequestService.getFriendRequests({ id, receiverId: this.authService.currentUser.userId })
        .subscribe(x => {
          const text = `${x[0].name} send you a friend request`
          this.notifications.push(text)
        })
    })

    this.signalR.setConnectionListener(friendConnectionId, 'GetRequestResponse', (x: FriendRequestResponse) => {
      const text = `${x.name} ${x.accepted ? 'accepted' : 'denied'} your request`
      this.notifications.push(text)
    })

    const directMessageConnectionId = await this.signalR.openConnection('direct-message')

    this.signalR.setConnectionListener(directMessageConnectionId,'GetMessage', (msg: DirectMessage) => {
      if (msg.senderId == this.authService.currentUser.userId) {
        return
      }
      this.notifications.push(`${msg.senderName} writes: ${msg.content}`)
    })
  }

  remove(index: number) {
    this.notifications.splice(index, 1)
  }
}
