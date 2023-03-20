import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  currentUser = ''
  authorized = false


  constructor(private authService: AuthService, private router: Router) {
    authService.onAuthChange().subscribe(res => {
      this.currentUser = res.userName
      this.authorized = res.userName != ''
    })
  }

  logout() {
    this.authService.logout()
    this.authService.clearUserData()
    this.router.navigateByUrl('/login')
  }
}
