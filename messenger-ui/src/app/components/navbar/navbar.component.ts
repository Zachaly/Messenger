import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { faBell } from '@fortawesome/free-solid-svg-icons'
import { NotificationService } from 'src/app/services/notification.service';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  currentUser = ''
  authorized = false
  bell = faBell
  showNotifications = false
  unreadNotifications: number = 0
  imageUrl = ''

  constructor(private authService: AuthService, private router: Router,
    private notificationService: NotificationService, private imageService: ImageService) {
    authService.onAuthChange().subscribe(res => {
      this.currentUser = res.userName
      this.authorized = res.userName != ''
      this.imageUrl = imageService.getUrl('profile', res.userId)
    })
    notificationService.notificationCountSubject.subscribe(res => this.unreadNotifications = this.showNotifications ? 0 : res)
  }

  toggleNotifications() {
    this.showNotifications = !this.showNotifications
    if (this.showNotifications) {
      this.unreadNotifications = 0
    }
  }

  logout() {
    this.authService.logout()
    this.authService.clearUserData()
    this.router.navigateByUrl('/login')
  }
}
