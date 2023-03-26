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
export class DirectChatPageComponent {

  userId?: number = 0
  currentUserId: number = 0

  messages: DirectMessage[] = []

  newMessageContent: string = ''

  constructor(private route: ActivatedRoute) {
    route.params.subscribe(param => {
      this.userId = param['userId']
    })
  }

}
