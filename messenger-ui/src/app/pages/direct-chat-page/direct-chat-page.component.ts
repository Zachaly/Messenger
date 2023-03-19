import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import DirectMessage from 'src/app/models/DirectMessage';
import AddDirectMessageRequest from 'src/app/requests/AddDirectMessageRequest';
import { AuthService } from 'src/app/services/auth.service';
import { DirectMessageService } from 'src/app/services/direct-message.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-direct-chat-page',
  templateUrl: './direct-chat-page.component.html',
  styleUrls: ['./direct-chat-page.component.css']
})
export class DirectChatPageComponent implements OnInit {

  userId: number = 0
  currentUserId: number = 0

  messages: DirectMessage[] = []

  newMessageContent: string = ''

  constructor(authSevice: AuthService, private messageService: DirectMessageService,
    private route: ActivatedRoute, private signalR: SignalrService) {
    this.currentUserId = authSevice.currentUser.userId
  }
  ngOnInit(): void {
    this.route.params.subscribe(param => {
      this.userId = param['userId']

      this.messageService
        .getMessages({ user1Id: this.currentUserId, user2Id: this.userId })
        .subscribe(res => this.messages.push(...res))

      this.signalR.openDirectMessageConnection()
      this.signalR.setDirectMessageConnectionListener('GetMessage', (msg: DirectMessage) => this.messages.push(msg))
    })
  }

  sendMessage() {
    const request: AddDirectMessageRequest = { senderId: this.currentUserId, receiverId: this.userId, content: this.newMessageContent }
    this.messageService.postMessage(request).subscribe(() => this.newMessageContent = '')
  }
}
