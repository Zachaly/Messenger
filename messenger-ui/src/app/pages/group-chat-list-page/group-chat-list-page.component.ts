import { Component, OnDestroy } from '@angular/core';
import ChatModel from 'src/app/models/ChatModel';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-group-chat-list-page',
  templateUrl: './group-chat-list-page.component.html',
  styleUrls: ['./group-chat-list-page.component.css'],
  providers: [SignalrService]
})
export class GroupChatListPageComponent implements OnDestroy {
  chats: ChatModel[] = []

  userId: number = 0
  selectedChat: ChatModel | null = null
  addingChat = false
  updatedChat: ChatModel | null = null

  constructor(private authService: AuthService, private chatService: ChatService, private signalR: SignalrService) {
    this.userId = authService.currentUser.userId
    this.loadChats()
    this.connectToWs()
  }
  ngOnDestroy(): void {
    this.signalR.stopAllConnections()
  }

  async connectToWs() {
    const chatConnection = await this.signalR.openConnection('chat')

    this.signalR.setConnectionListener(chatConnection, 'ChatUpdated', (chat: ChatModel) => {
      const currentChat = this.chats.find(x => x.id == chat.id)
      if(currentChat){
        currentChat.name = chat.name
      }
    })
  }

  loadChats() {
    this.chatService.getChat({ userId: this.userId }).subscribe(res => this.chats = res)
  }

  onSubmitChat(){
    this.addingChat = false
    this.loadChats()
  }

  selectChat(chat: ChatModel){
    this.selectedChat = chat
    this.updatedChat = null
  }

  updateChat() {
    this.updatedChat = null
  }
}
