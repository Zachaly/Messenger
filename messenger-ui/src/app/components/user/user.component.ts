import { Component, EventEmitter, Input, Output } from '@angular/core';
import UserListItem from 'src/app/models/UserListItem';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  @Input() user: UserListItem = { id: 0, name: ''}
  @Input() isFriend: boolean = false
  @Output() addFriend: EventEmitter<any> = new EventEmitter()

  constructor(private imageService: ImageService) {

  }

  onAddFriend(){
    this.addFriend.emit()
  }

  getImageUrl() : string {
    return this.imageService.getUrl('profile', this.user.id)
  }
}
