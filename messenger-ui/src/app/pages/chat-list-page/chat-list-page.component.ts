import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import UserListItem from 'src/app/models/UserListItem';
import { AuthService } from 'src/app/services/auth.service';
import { DirectMessageService } from 'src/app/services/direct-message.service';
import { FriendService } from 'src/app/services/friend.service';

@Component({
  selector: 'app-chat-list-page',
  templateUrl: './chat-list-page.component.html',
  styleUrls: ['./chat-list-page.component.css'],
})
export class ChatListPageComponent implements OnInit {

  chats: [UserListItem, number][] = []
  selectedChat?: UserListItem = undefined

  constructor(private authService: AuthService, private friendService: FriendService,
    private directMessageService: DirectMessageService) {
  }
  
  ngOnInit(): void {
    this.friendService.getFriends(this.authService.currentUser.userId).subscribe(res => {
      res.forEach(chat => {
        const countRequest = { receiverId: this.authService.currentUser.userId, senderId: chat.id, read: false }
        this.directMessageService.getMessageCount(countRequest).subscribe(count => {
          this.chats.push([chat, count])
        })
      })
    })
  }

  selectChat(user: UserListItem): void {
    this.selectedChat = user
  }

  onMessageRead(chat: UserListItem) {
    this.chats.find(x => x[0] == chat)![1]--
  }
}
