import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import ChatUserModel from 'src/app/models/ChatUserModel';
import AddChatUserRequest from 'src/app/requests/AddChatUserRequest';
import GetChatUserRequest from 'src/app/requests/GetChatUserRequest';
import UpdateChatUserRequest from 'src/app/requests/UpdateChatUserRequest';
import { AuthService } from 'src/app/services/auth.service';
import { ChatUserService } from 'src/app/services/chat-user.service';
import { SignalrService } from 'src/app/services/signalr.service';

const PAGE_SIZE = 10

@Component({
  selector: 'app-chat-user-list',
  templateUrl: './chat-user-list.component.html',
  styleUrls: ['./chat-user-list.component.css'],
  providers: [SignalrService]
})
export class ChatUserListComponent implements OnChanges, OnDestroy {
  @Input() chatId: number = 0
  @Input() creatorId: number = 0

  currentUserId: number = 0

  count = 0
  nextPage = 0
  maxPage = (): number => Math.floor(this.count / PAGE_SIZE)
  users: ChatUserModel[] = []
  userIds: number[] = []
  addingUser = false
  loading = false

  constructor(private chatUserService: ChatUserService, private authService: AuthService, private signalR: SignalrService) {
    this.currentUserId = authService.currentUser.userId
  }
  ngOnDestroy(): void {
    this.signalR.stopAllConnections()
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.load()
  }

  async load() {
    this.users = []
    this.nextPage = 0
    this.loading = false
    const request: GetChatUserRequest = { chatId: this.chatId }
    this.signalR.stopAllConnections()

    const chatConnectionId = await this.signalR.openConnection('chat')

    this.signalR.setConnectionListener(chatConnectionId, 'ChatUserAdded', (user: ChatUserModel) => {
      this.users.push(user)
    })

    this.signalR.setConnectionListener(chatConnectionId, 'ChatUserRemoved', (id: number) => {
      this.users = this.users.filter(user => user.id !== id)
    })

    this.signalR.setConnectionListener(chatConnectionId, 'ChatUserUpdated', (user: ChatUserModel) => {
      const index = this.users.findIndex(u => u.id == user.id)
      if(index >= 0) {
        this.users.splice(index, 1, user)
      }
    })

    this.chatUserService.getChatUserCount(request).subscribe(res => {
      this.count = res
      this.chatUserService.getChatUser({ chatId: this.chatId, pageSize: this.count}).subscribe(res => {
        this.userIds = res.map(x => x.id)
      })
    })
    this.loadNext()
  }

  loadNext() {
    if (this.loading || this.nextPage > this.maxPage()) {
      return
    }
    this.loading = true
    const request: GetChatUserRequest = { chatId: this.chatId, pageSize: PAGE_SIZE, pageIndex: this.nextPage }
    this.chatUserService.getChatUser(request).subscribe(res => {
      this.users.push(...res)
      this.loading = false
      this.nextPage++
    })
  }

  addUser(id: number) {
    const addUserRequest: AddChatUserRequest = { chatId: this.chatId, userId: id }
    this.chatUserService.addChatUser(addUserRequest).subscribe(() => {
      this.load()
    })
  }

  isAdmin = (): boolean => this.users.find(x => x.id == this.currentUserId)?.isAdmin ?? false

  canRemove = (user: ChatUserModel) => (user.id == this.currentUserId || this.isAdmin()) && !user.isAdmin

  removeUser(user: ChatUserModel) {
    this.chatUserService.deleteChatUser(this.chatId, user.id).subscribe(() => {
      this.users = this.users.filter(x => x.id !== user.id)
      this.count--
    })
  }

  makeAdmin(user: ChatUserModel) {
    const request: UpdateChatUserRequest = { chatId: this.chatId, userId: user.id, isAdmin: true }
    this.chatUserService.updateChatUserRequest(request).subscribe(() => {
      this.load()
    })
  }

  onScroll(event: Event) {
    const target = event.target as HTMLDivElement
    if (target.scrollTop > target.scrollHeight * 0.5 && !this.loading) {
      this.loadNext()
    }
  }
}
