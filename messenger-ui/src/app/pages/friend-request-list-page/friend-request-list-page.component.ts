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
    this.loadRequests()
  }

  loadRequests() {
    if(this.tabIndex == 0){
      this.friendRequestService.getFriendRequests({ receiverId: this.authService.currentUser.userId }).subscribe(res => this.requests = res)
    } else if(this.tabIndex == 1) {
      this.friendRequestService.getFriendRequests({ senderId: this.authService.currentUser.userId }).subscribe(res => this.requests = res)
    }
  }

  respond(requestId: number, accepted: boolean) {
    const request: RespondToFriendRequestRequest = {
      requestId,
      accepted
    }
    this.friendRequestService.respondToFriendRequest(request).subscribe(res => this.requests = this.requests.filter(x => x.id !== requestId))
  }

  cancel(id: number) {
    this.friendRequestService.delete(id).subscribe(res => this.requests = this.requests.filter(x => x.id !== id))
  }
}
