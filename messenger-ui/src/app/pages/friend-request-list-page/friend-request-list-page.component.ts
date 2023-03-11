import { Component, OnInit } from '@angular/core';
import FriendRequest from 'src/app/models/FriendRequest';
import RespondToFriendRequestRequest from 'src/app/requests/RespondToFriendRequestRequest';
import { AuthService } from 'src/app/services/auth.service';
import { FriendRequestService } from 'src/app/services/friend-request.service';

@Component({
  selector: 'app-friend-request-list-page',
  templateUrl: './friend-request-list-page.component.html',
  styleUrls: ['./friend-request-list-page.component.css']
})
export class FriendRequestListPageComponent implements OnInit {
  tabs = ['Received', 'Sent']
  tabIndex = 0

  requests: FriendRequest[] = []

  constructor(private authService: AuthService, private friendRequestService: FriendRequestService) {

  }

  ngOnInit(): void {
    this.onTabChange(0)
  }

  onTabChange(index: number){
    this.tabIndex = index

    if(index == 0){
      this.friendRequestService.getFriendRequests({ receiverId: this.authService.currentUser.userId }).subscribe(res => this.requests = res)
    } else if(index == 1) {
      this.friendRequestService.getFriendRequests({ senderId: this.authService.currentUser.userId }).subscribe(res => this.requests = res)
    }
  }

  respond(requestId: number, accepted: boolean) {
    const request: RespondToFriendRequestRequest = {
      requestId,
      accepted
    }
    this.friendRequestService.respondToFriendRequest(request).subscribe()
  }
}
