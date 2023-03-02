import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import UserListItem from 'src/app/models/UserListItem';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {
  users: UserListItem[] = []

  constructor(private authService: AuthService, private router: Router,
    private userService: UserService) {
    if (!authService.currentUser.authToken) {
      router.navigateByUrl('/login')
    }
  }
  ngOnInit(): void {
    this.userService.getUsers({})
      .subscribe(res => this.users = res)
  }
}
