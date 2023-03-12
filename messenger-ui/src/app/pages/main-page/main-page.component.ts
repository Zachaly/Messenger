import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import UserListItem from 'src/app/models/UserListItem';
import AddFriendRequest from 'src/app/requests/AddFriendRequest';
import { AuthService } from 'src/app/services/auth.service';
import { FriendRequestService } from 'src/app/services/friend-request.service';
import { FriendService } from 'src/app/services/friend.service';
import { SignalrService } from 'src/app/services/signalr.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {
  users: UserListItem[] = []
  friends: UserListItem[] = []

  constructor(private authService: AuthService, private router: Router,
    private userService: UserService, private friendService: FriendService,
    private friendRequestService: FriendRequestService) {
    if (!authService.currentUser.authToken) {
      router.navigateByUrl('/login')
    }
  }
  ngOnInit(): void {
    this.userService.getUsers({})
      .subscribe(res => this.users = res.filter(x => x.id !== this.authService.currentUser.userId))
    this.friendService.getFriends(this.authService.currentUser.userId).subscribe(res => this.friends = res)
  }

  isFriend(userId: number): boolean {
    return this.friends.some(x => x.id == userId)
  }

  addFriend(userId: number) {
    const request: AddFriendRequest = {
      senderId: this.authService.currentUser.userId,
      receiverId: userId
    }

    this.friendRequestService.postFriendRequest(request).subscribe({
      next: () => alert('Friend request send'),
      error: (err) => alert(err.error.error)
    })
  }
}
