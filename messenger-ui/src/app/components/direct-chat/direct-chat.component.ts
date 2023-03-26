import { Component, Input, OnInit, OnDestroy, ChangeDetectionStrategy, OnChanges, SimpleChanges, ChangeDetectorRef } from '@angular/core';
import DirectMessage from 'src/app/models/DirectMessage';
import AddDirectMessageRequest from 'src/app/requests/AddDirectMessageRequest';
import UpdateDirectMessageRequest from 'src/app/requests/UpdateDirectMessageRequest';
import { AuthService } from 'src/app/services/auth.service';
import { DirectMessageService } from 'src/app/services/direct-message.service';
import { SignalrService } from 'src/app/services/signalr.service';

const PAGE_SIZE = 20

@Component({
  selector: 'app-direct-chat',
  templateUrl: './direct-chat.component.html',
  styleUrls: ['./direct-chat.component.css'],
})
export class DirectChatComponent implements OnInit, OnDestroy, OnChanges {

  @Input() userId: number = 0
  currentUserId: number = 0

  messages: DirectMessage[] = []

  newMessageContent: string = ''

  constructor(authSevice: AuthService, private messageService: DirectMessageService, private signalR: SignalrService) {
    this.currentUserId = authSevice.currentUser.userId
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.messages = []
    this.loadChat()
  }

  ngOnDestroy(): void {
    this.signalR.stopAllConnections()
  }

  ngOnInit() {
    this.loadChat()
  }

  loadChat() {
    this.signalR.stopAllConnections()

    this.messageService
      .getMessages({ user1Id: this.currentUserId, user2Id: this.userId })
      .subscribe(res => res.forEach(msg => this.readMessage(msg)))

    this.signalR.openConnection('direct-message').then(directMessageConnectionId => {
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

  private readMessage(msg: DirectMessage) {
    this.messages.push(msg)
    if (msg.senderId == this.currentUserId) {
      return;
    }

    if (!msg.read) {
      const updateRequest: UpdateDirectMessageRequest = { id: msg.id, read: true }
      msg.read = true;
      this.messageService.updateMessage(updateRequest).subscribe();
    }
  }
}
