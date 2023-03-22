import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import DirectMessage from 'src/app/models/DirectMessage';
import AddDirectMessageRequest from 'src/app/requests/AddDirectMessageRequest';
import UpdateDirectMessageRequest from 'src/app/requests/UpdateDirectMessageRequest';
import { AuthService } from 'src/app/services/auth.service';
import { DirectMessageService } from 'src/app/services/direct-message.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-direct-chat-page',
  templateUrl: './direct-chat-page.component.html',
  styleUrls: ['./direct-chat-page.component.css'],
  providers: [SignalrService]
})
export class DirectChatPageComponent implements OnInit, OnDestroy {

  userId: number = 0
  currentUserId: number = 0

  messages: DirectMessage[] = []

  newMessageContent: string = ''

  constructor(authSevice: AuthService, private messageService: DirectMessageService,
    private route: ActivatedRoute, private signalR: SignalrService) {
    this.currentUserId = authSevice.currentUser.userId
  }

  ngOnDestroy(): void {
   this.signalR.stopAllConnections()
  }

  ngOnInit(): void {
    this.route.params.subscribe(async param => {
      this.userId = param['userId']

      this.messageService
        .getMessages({ user1Id: this.currentUserId, user2Id: this.userId })
        .subscribe(res => res.forEach(msg => this.readMessage(msg)))

      const directMessageConnectionId = await this.signalR.openConnection('direct-message')

      this.signalR.setConnectionListener(directMessageConnectionId, 'GetMessage', (msg: DirectMessage) => this.readMessage(msg))
      this.signalR.setConnectionListener(directMessageConnectionId, 'ReadMessage', (x: number, read: boolean) => {
        this.messages.find(msg => msg.id === x)!.read = read
      })
    })
  }

  sendMessage() {
    const request: AddDirectMessageRequest = { senderId: this.currentUserId, receiverId: this.userId, content: this.newMessageContent }
    this.messageService.postMessage(request).subscribe(() => this.newMessageContent = '')
  }

  readMessage(msg: DirectMessage) {
    this.messages.push(msg)

    if(msg.senderId == this.currentUserId) {
      return;
    }

    if(!msg.read) {
      const updateRequest: UpdateDirectMessageRequest = { id: msg.id, read: true }
      msg.read = true;
      this.messageService.updateMessage(updateRequest).subscribe();
    }
  }
}
