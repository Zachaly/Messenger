import { Component } from '@angular/core';
import ChatMessageModel from 'src/app/models/ChatMessageModel';
import ChatUserModel from 'src/app/models/ChatUserModel';
import DirectMessage from 'src/app/models/DirectMessage';
import FriendRequestResponse from 'src/app/models/FriendRequestResponse';
import NotificationModel from 'src/app/models/NotificationModel';
import { AuthService } from 'src/app/services/auth.service';
import { FriendRequestService } from 'src/app/services/friend-request.service';
import { NotificationService } from 'src/app/services/notification.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css'],
  providers: [SignalrService]
})
export class NotificationListComponent {

  notifications: NotificationModel[] = []

  constructor(private authService: AuthService, private signalR: SignalrService, private friendRequestService: FriendRequestService,
    private notificationService: NotificationService) {
    notificationService.notificationSubject.subscribe(res => this.notifications = res)
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
          this.notificationService.addNotification(text)
        })
    })

    this.signalR.setConnectionListener(friendConnectionId, 'GetRequestResponse', (x: FriendRequestResponse) => {
      const text = `${x.name} ${x.accepted ? 'accepted' : 'denied'} your request`
      this.notificationService.addNotification(text)
    })

    const directMessageConnectionId = await this.signalR.openConnection('direct-message')

    this.signalR.setConnectionListener(directMessageConnectionId, 'GetMessage', (msg: DirectMessage) => {
      if (msg.senderId == this.authService.currentUser.userId) {
        return
      }
      this.notificationService.addNotification(`${msg.senderName} writes: ${msg.content}`)
    })

    const chatConnectionId = await this.signalR.openConnection('chat')

    this.signalR.setConnectionListener(chatConnectionId, 'ChatUserAdded', (user: ChatUserModel) => {
      if (user.id == this.authService.currentUser.userId) {
        this.notificationService.addNotification("You are added to new chat!")
      }
    })

    this.signalR.setConnectionListener(chatConnectionId, 'ChatUserRemoved', (id: number) => {
      if(id === this.authService.currentUser.userId) {
        this.notificationService.addNotification("You are removed from chat")
      }
    })

    this.signalR.setConnectionListener(chatConnectionId, 'ChatMessageSend', (msg: ChatMessageModel) => {
      if(msg.senderId !== this.authService.currentUser.userId){
        this.notificationService.addNotification(`${msg.senderName} wrote new message in chat!`)
      }
    })

    const claimConnectionId = await this.signalR.openConnection('claim')

    this.signalR.setConnectionListener(claimConnectionId, 'ClaimAdded', (claim: string) => {
      this.notificationService.addNotification(`You are given ${claim} rights`)
    })
  }

  remove(index: number) {
    this.notifications.splice(index, 1)
  }
}
