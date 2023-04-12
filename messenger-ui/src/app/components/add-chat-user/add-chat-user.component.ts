import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import UserListItem from 'src/app/models/UserListItem';
import AddChatUserRequest from 'src/app/requests/AddChatUserRequest';
import { ChatUserService } from 'src/app/services/chat-user.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-add-chat-user',
  templateUrl: './add-chat-user.component.html',
  styleUrls: ['./add-chat-user.component.css']
})
export class AddChatUserComponent implements OnInit {
  @Input() chatId: number = 0
  @Input() currentChatUsers: number[] = []
  users: UserListItem[] = []

  @Output() addUser: EventEmitter<number> = new EventEmitter()
  @Output() cancel: EventEmitter<any> = new EventEmitter()

  constructor(private userService: UserService, private chatUserService: ChatUserService) { }

  ngOnInit(): void {
    this.userService.getUsers({})
      .subscribe(res => this.users = res.filter(x => !this.currentChatUsers.includes(x.id)))
  }

  selectUser(user: UserListItem) {
    this.addUser.emit(user.id)
    this.users = this.users.filter(x => x.id !== user.id)
  }

  onCancel() {
    this.cancel.emit()
  }
}
