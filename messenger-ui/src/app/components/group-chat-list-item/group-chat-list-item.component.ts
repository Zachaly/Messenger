import { Component, EventEmitter, Input, Output } from '@angular/core';
import ChatModel from 'src/app/models/ChatModel';

@Component({
  selector: 'app-group-chat-list-item',
  templateUrl: './group-chat-list-item.component.html',
  styleUrls: ['./group-chat-list-item.component.css']
})
export class GroupChatListItemComponent {
  @Input() chat: ChatModel = { id: 0, creatorId: 0, name: '', creatorName: ''}
  @Output() selectChat: EventEmitter<number> = new EventEmitter()

  onClick() {
    this.selectChat.emit(this.chat.id)
  }
}
