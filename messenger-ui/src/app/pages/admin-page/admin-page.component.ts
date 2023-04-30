import { Component, OnInit } from '@angular/core';
import UserListItem from 'src/app/models/UserListItem';
import { UserClaimService } from 'src/app/services/user-claim.service';
import { UserService } from 'src/app/services/user.service';

const MODERATOR_CLAIM = 'Moderator'

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.css']
})
export class AdminPageComponent implements OnInit {

  moderators: UserListItem[] = []
  users: UserListItem[] = []
  searchName = ''

  constructor(private userService: UserService, private userClaimService: UserClaimService) {

  }

  ngOnInit(): void {
    this.loadUsers()
  }

  addModerator(id: number): void {
    this.userClaimService.addUserClaim({ value: MODERATOR_CLAIM, userId: id })
      .subscribe(() => this.loadUsers())
  }

  loadUsers(): void {
    this.userService.getUsers({ claimValue: MODERATOR_CLAIM }).subscribe(res => {
      this.moderators = res
      this.users = []
    })
  }

  removeModerator(id: number): void {
    this.userClaimService.deleteUserClaim(id, MODERATOR_CLAIM)
      .subscribe(() => this.loadUsers())
  }

  searchUsers() {
    this.userService.getUsers({ searchName: this.searchName })
      .subscribe(res => this.users = res.filter(x => !this.moderators.some(y => y.id == x.id)))
  }
}
