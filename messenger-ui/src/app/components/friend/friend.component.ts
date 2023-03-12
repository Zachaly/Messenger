import { Component, EventEmitter, Input, Output } from '@angular/core';
import UserListItem from 'src/app/models/UserListItem';

@Component({
  selector: 'app-friend',
  templateUrl: './friend.component.html',
  styleUrls: ['./friend.component.css']
})
export class FriendComponent {
  @Input() user: UserListItem = { id: 0, name: ''}
  @Output() delete: EventEmitter<any> = new EventEmitter()

  onDeleteClick() {
    this.delete.emit()
  }
}
