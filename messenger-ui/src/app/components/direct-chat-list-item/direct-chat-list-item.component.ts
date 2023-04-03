import { Component, EventEmitter, Input, Output } from '@angular/core';
import UserListItem from 'src/app/models/UserListItem';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-direct-chat-list-item',
  templateUrl: './direct-chat-list-item.component.html',
  styleUrls: ['./direct-chat-list-item.component.css']
})
export class DirectChatListItemComponent {
  @Input() user: UserListItem = { id: 0, name: '' }
  @Output() selectChat: EventEmitter<any> = new EventEmitter()
  @Input() unreadMessages: number = 0

  constructor(private imageService: ImageService) {

  }

  onSelect() {
    this.selectChat.emit()
  }

  imageUrl = () => this.imageService.getUrl('profile', this.user.id)
}
