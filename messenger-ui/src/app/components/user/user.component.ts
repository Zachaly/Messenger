import { Component, EventEmitter, Input, Output } from '@angular/core';
import UserListItem from 'src/app/models/UserListItem';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  @Input() user: UserListItem = { id: 0, name: ''}
  @Input() isFriend: boolean = false
  @Output() addFriend: EventEmitter<any> = new EventEmitter()

  onAddFriend(){
    this.addFriend.emit()
  }
}
