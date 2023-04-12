import { Component, EventEmitter, Input, Output } from '@angular/core';
import ChatUserModel from 'src/app/models/ChatUserModel';

@Component({
  selector: 'app-chat-user',
  templateUrl: './chat-user.component.html',
  styleUrls: ['./chat-user.component.css']
})
export class ChatUserComponent {
  @Input() user: ChatUserModel = { id: 0, name: '', isAdmin: false }
  @Input() removable: boolean = false
  @Input() isCurrentUserCreator: boolean = false
  @Output() remove: EventEmitter<any> = new EventEmitter()
  @Output() makeAdmin: EventEmitter<any> = new EventEmitter()
}
