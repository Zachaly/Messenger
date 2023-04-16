import { Component, ElementRef, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import ChatMessageModel from 'src/app/models/ChatMessageModel';
import ChatUserModel from 'src/app/models/ChatUserModel';
import AddChatMessageReadRequest from 'src/app/requests/AddChatMessageReadRequest';
import AddChatMessageRequest from 'src/app/requests/AddChatMessageRequest';
import GetChatMessageRequest from 'src/app/requests/GetChatMessageRequest';
import { AuthService } from 'src/app/services/auth.service';
import { ChatMessageReadService } from 'src/app/services/chat-message-read.service';
import { ChatMessageService } from 'src/app/services/chat-message.service';
import { ImageService } from 'src/app/services/image.service';
import { SignalrService } from 'src/app/services/signalr.service';

const PAGE_SIZE = 10

@Component({
  selector: 'app-group-chat',
  templateUrl: './group-chat.component.html',
  styleUrls: ['./group-chat.component.css'],
  providers: [SignalrService]
})
export class GroupChatComponent implements OnChanges, OnDestroy {
  @Input() chatId: number = 0
  @Input() creatorId: number = 0
  @ViewChild('messageBox') messageBox!: ElementRef<HTMLDivElement>
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>

  messages: ChatMessageModel[] = []
  userId = 0
  nextPage = 0
  count = 0
  maxPage = (): number => Math.floor(this.count / PAGE_SIZE)

  selectedFiles: FileList | null = null

  loading = false
  firstScrolled = false

  newMessageContent = ''

  constructor(private authService: AuthService, private chatMessageService: ChatMessageService,
    private chatMessageReadService: ChatMessageReadService, private signalR: SignalrService,
    private imageService: ImageService) {
    this.userId = authService.currentUser.userId
  }
  
  ngOnDestroy(): void {
    this.signalR.stopAllConnections()
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.load()
  }

  async load() {
    this.nextPage = 0
    this.messages = []
    this.firstScrolled = false
    this.signalR.stopAllConnections()

    const chatConnectionId = await this.signalR.openConnection('chat')

    this.signalR.setConnectionListener(chatConnectionId, 'ChatMessageSend', (msg: ChatMessageModel) => {
      this.readMessage(msg, true)
    })

    this.signalR.setConnectionListener(chatConnectionId, 'ChatMessageRead', (messageId: number, userId: number) => {
      const msg = this.messages.find(x => x.id == messageId)
      if(msg){
        msg.readByIds.push(userId)
      }
    })

    const request: GetChatMessageRequest = { chatId: this.chatId }
    this.chatMessageService.getChatMessageCount(request).subscribe(res => this.count = res)
    this.loadNext()
  }

  loadNext() {
    if (this.loading || this.nextPage > this.maxPage()) {
      return
    }
    const request: GetChatMessageRequest = { chatId: this.chatId, pageIndex: this.nextPage, pageSize: PAGE_SIZE }

    this.loading = true
    this.chatMessageService.getChatMessage(request).subscribe(res => {
      res.forEach(msg => this.readMessage(msg))
      this.nextPage++
      this.loading = false
    })
  }

  sendMessage() {
    const request: AddChatMessageRequest = { chatId: this.chatId, senderId: this.userId, content: this.newMessageContent }

    this.chatMessageService.addChatMessage(request).subscribe(res => {
      
      if(this.selectedFiles) {
        const messageId: number = res.newEntityId!
        this.imageService.uploadChatMessageImages(this.selectedFiles, messageId).subscribe(() => {
          this.chatMessageService.getChatMessage({ id: messageId }).subscribe(res => {
            const message = res[0]!
            this.messages.find(x => x.id == messageId)!.imageIds = message.imageIds
            this.selectedFiles = null
            this.fileInput.nativeElement.files = null
          })
        })
      }
      this.newMessageContent = ''
      this.scrollBottom()
    })
  }

  private scrollBottom() {
    this.messageBox.nativeElement.scrollTop = this.messageBox.nativeElement.scrollHeight
  }

  readMessage(msg: ChatMessageModel, push?: boolean) {
    if (this.messages.some(x => x.id == msg.id)) {
      return
    }

    push ? this.messages.push(msg) : this.messages.unshift(msg)

    if (msg.senderId == this.userId) {
      return
    }

    if (!msg.readByIds.some(x => x == this.userId)) {
      console.log('read')
      const request: AddChatMessageReadRequest = { userId: this.userId, messageId: msg.id, chatId: this.chatId }
      this.chatMessageReadService.addChatMessageRead(request).subscribe()
    }
  }

  ngAfterViewChecked() {
    if (!this.firstScrolled && !this.loading && this.messageBox.nativeElement.scrollHeight > 0) {
      this.scrollBottom()
      this.firstScrolled = true
    }
  }

  onScroll(event: Event) {
    const target = event.target as HTMLDivElement

    if (target.scrollTop < target.scrollHeight / 20 && !this.loading) {
      this.loadNext()
    }
  }

  selectImages() {
    this.selectedFiles = this.fileInput.nativeElement.files
  }
}
