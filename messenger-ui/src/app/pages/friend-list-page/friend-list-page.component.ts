import { Component, OnInit } from '@angular/core';
import UserListItem from 'src/app/models/UserListItem';
import { AuthService } from 'src/app/services/auth.service';
import { FriendService } from 'src/app/services/friend.service';

@Component({
  selector: 'app-friend-list-page',
  templateUrl: './friend-list-page.component.html',
  styleUrls: ['./friend-list-page.component.css']
})
export class FriendListPageComponent implements OnInit {
  friends: UserListItem[] = []

  constructor(private friendService: FriendService, private authService: AuthService) { }
  ngOnInit(): void {
    this.friendService.getFriends(this.authService.currentUser.userId).subscribe(res => this.friends = res)
  }


}
