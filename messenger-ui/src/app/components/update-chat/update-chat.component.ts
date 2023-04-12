import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import ChatModel from 'src/app/models/ChatModel';
import UpdateChatRequest from 'src/app/requests/UpdateChatRequest';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';

@Component({
  selector: 'app-update-chat',
  templateUrl: './update-chat.component.html',
  styleUrls: ['./update-chat.component.css']
})
export class UpdateChatComponent implements OnInit {
  @Input() chat: ChatModel = { id: 0, name: '', creatorId: 0, creatorName: '' }

  request: UpdateChatRequest = { name: '', id: 0 }

  @Output() submit: EventEmitter<any> = new EventEmitter()
  @Output() cancel: EventEmitter<any> = new EventEmitter()

  constructor(private chatService: ChatService) {

  }
  ngOnInit(): void {
    this.request.id = this.chat.id
    this.request.name = this.chat.name
  }

  onSubmit() {
    this.chatService.updateChat(this.request).subscribe(res => {
      alert('Chat updated!')
      this.submit.emit()
    })
  }

  onCancel() {
    this.cancel.emit()
  }
}
