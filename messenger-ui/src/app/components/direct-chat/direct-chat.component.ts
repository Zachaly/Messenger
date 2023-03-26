import { Component, Input, OnInit, OnDestroy, OnChanges, SimpleChanges, ViewChild, ElementRef, AfterViewChecked, Output, EventEmitter } from '@angular/core';
import DirectMessage from 'src/app/models/DirectMessage';
import AddDirectMessageRequest from 'src/app/requests/AddDirectMessageRequest';
import UpdateDirectMessageRequest from 'src/app/requests/UpdateDirectMessageRequest';
import { AuthService } from 'src/app/services/auth.service';
import { DirectMessageService } from 'src/app/services/direct-message.service';
import { SignalrService } from 'src/app/services/signalr.service';

const PAGE_SIZE = 5

@Component({
  selector: 'app-direct-chat',
  templateUrl: './direct-chat.component.html',
  styleUrls: ['./direct-chat.component.css'],
})
export class DirectChatComponent implements OnInit, OnDestroy, OnChanges, AfterViewChecked {

  @ViewChild('messageBox') messageBox!: ElementRef<HTMLDivElement>

  @Input() userId: number = 0
  currentUserId: number = 0

  @Output() messageRead: EventEmitter<any> = new EventEmitter()

  maxPage = 0
  nextPage = 0
  loading = false

  firstScrolled = false

  messages: DirectMessage[] = []

  newMessageContent: string = ''

  constructor(authSevice: AuthService, private messageService: DirectMessageService, private signalR: SignalrService) {
    this.currentUserId = authSevice.currentUser.userId
  }

  ngAfterViewChecked() {
    if (!this.firstScrolled && !this.loading && this.messageBox.nativeElement.scrollHeight > 0) {
      this.scrollBottom()
      this.firstScrolled = true
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.messages = []
    this.nextPage = 0
    this.firstScrolled = false
    this.loadChat()
  }

  ngOnDestroy(): void {
    this.signalR.stopAllConnections()
  }

  ngOnInit() {
    this.loadChat()
  }

  private readMessage(msg: DirectMessage, push = true) {
    if (this.messages.some(x => x.id == msg.id)) {
      return
    }

    push ? this.messages.push(msg) : this.messages.unshift(msg)

    if (msg.senderId == this.currentUserId) {
      return;
    }

    if (!msg.read) {
      const updateRequest: UpdateDirectMessageRequest = { id: msg.id, read: true }
      msg.read = true;
      this.messageService.updateMessage(updateRequest).subscribe();
      this.messageRead.emit()
    }
  }

  private scrollBottom() {
    this.messageBox.nativeElement.scrollTop = this.messageBox.nativeElement.scrollHeight
  }

  private loadMessages() {
    if (this.nextPage >= this.maxPage) {
      return
    }
    const pageIndex = this.nextPage

    this.loading = true
    this.messageService
      .getMessages({ user1Id: this.currentUserId, user2Id: this.userId, pageSize: PAGE_SIZE, pageIndex })
      .subscribe(res => {
        res.forEach(msg => this.readMessage(msg, false))
        this.loading = false
      })

    this.nextPage++
  }

  private loadChat() {
    this.signalR.stopAllConnections()

    this.messageService.getMessageCount({ user1Id: this.currentUserId, user2Id: this.userId }).subscribe(count => {
      this.maxPage = Math.ceil(count / PAGE_SIZE)
      this.loadMessages()
    })

    this.signalR.openConnection('direct-message').then(directMessageConnectionId => {
      this.signalR.setConnectionListener(directMessageConnectionId, 'GetMessage', (msg: DirectMessage) => {
        this.readMessage(msg)
        this.scrollBottom()
      })
      this.signalR.setConnectionListener(directMessageConnectionId, 'ReadMessage', (x: number, read: boolean) => {
        this.messages.find(msg => msg.id === x)!.read = read
      })
    })
  }

  sendMessage() {
    const request: AddDirectMessageRequest = { senderId: this.currentUserId, receiverId: this.userId, content: this.newMessageContent }
    this.messageService.postMessage(request).subscribe(() => {
      this.newMessageContent = ''
      this.scrollBottom()
    })
  }

  onScroll(event: Event) {
    const target = event.target as HTMLDivElement

    if (target.scrollTop < target.scrollHeight / 20 && !this.loading) {
      this.loadMessages()
    }
  }
}
