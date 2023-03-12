import { Component, EventEmitter, Input, Output } from '@angular/core';
import FriendRequest from 'src/app/models/FriendRequest';

@Component({
  selector: 'app-friend-request',
  templateUrl: './friend-request.component.html',
  styleUrls: ['./friend-request.component.css']
})
export class FriendRequestComponent {
  @Input() request: FriendRequest = { id: 0, userId: 0, name: '' }
  @Input() receiver: boolean = false
  @Output() accept: EventEmitter<boolean> = new EventEmitter()
  @Output() cancel: EventEmitter<any> = new EventEmitter()


  onAccept(accepted: boolean){
    this.accept.emit(accepted)
  }

  onCancel() {
    this.cancel.emit()
  }
}
