import { Component, EventEmitter, Input, Output } from '@angular/core';
import UserBan from 'src/app/models/UserBan';

@Component({
  selector: 'app-banned-user',
  templateUrl: './banned-user.component.html',
  styleUrls: ['./banned-user.component.css']
})
export class BannedUserComponent {
  @Input() user!: UserBan
  @Output() liftBan: EventEmitter<any> = new EventEmitter()
}
