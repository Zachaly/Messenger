import { Component, OnInit } from '@angular/core';
import UserListItem from 'src/app/models/UserListItem';
import { AuthService } from 'src/app/services/auth.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-online-friends-bar',
  templateUrl: './online-friends-bar.component.html',
  styleUrls: ['./online-friends-bar.component.css'],
  providers: [SignalrService]
})
export class OnlineFriendsBarComponent implements OnInit {

  friends: UserListItem[] = []
  authorized = false

  constructor(private signalR: SignalrService, private authService: AuthService) {

  }
  ngOnInit(): void {
    if (this.authService.currentUser.userId) {
      this.setup()
    }
    this.authService.onAuthChange().subscribe(res => {
      this.authorized = res.userId != 0
      if (res.userId) {
        this.setup()
      } else {
        this.signalR.stopAllConnections()
      }
    })
  }

  async setup() {
    this.signalR.stopAllConnections()
    const connectionId = await this.signalR.openConnection('online-status')

    this.signalR.setConnectionListener(connectionId, 'FriendConnected', (friend: UserListItem) => {
      this.friends.push(friend)
    })

    this.signalR.setConnectionListener(connectionId, 'FriendDisconnected', (id: number) => {
      this.friends = this.friends.filter(x => x.id !== id)
    })

    this.signalR.invoke<UserListItem[]>(connectionId, 'GetOnlineFriends').then(res => {
      this.friends.push(...res)
    })
  }

}
