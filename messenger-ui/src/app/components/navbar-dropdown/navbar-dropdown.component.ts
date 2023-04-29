import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-navbar-dropdown',
  templateUrl: './navbar-dropdown.component.html',
  styleUrls: ['./navbar-dropdown.component.css']
})
export class NavbarDropdownComponent implements OnInit {
  @Input() user = ''
  @Input() userId = 0
  @Output() logout: EventEmitter<any> = new EventEmitter()

  isActive = false
  imageUrl = ''

  routes: [string, string][] = [
    ['Chats', '/chats'],
    ['Group chats', '/group-chats'],
    ['Friend Request', '/friend-request'],
    ['Friends', '/friend'],
    ['Update profile', '/update-profile'],
  ]

  constructor(private router: Router, private imageService: ImageService, private authService: AuthService) {
    authService.onAuthChange().subscribe(user => {
      console.log(user, 'change')
      if (user.claims.includes('Admin')) {
        this.routes.push(['Admin', '/admin'])
      }

      if(user.claims.includes('Admin') || user.claims.includes('Moderator')){
        this.routes.push(['Moderation', '/moderation'])
      }
    })
  }
  ngOnInit(): void {
    this.imageUrl = this.imageService.getUrl('profile', this.userId)
    const user = this.authService.currentUser
    console.log(user)

    if (user.claims.includes('Admin')) {
      this.routes.push(['Admin', '/admin'])
    }

    if(user.claims.includes('Admin') || user.claims.includes('Moderator')){
      this.routes.push(['Moderation', '/moderation'])
    }
  }

  onLogout() {
    this.isActive = false
    this.router.navigateByUrl('/login')
    this.logout.emit()
  }

  isCurrentRoute = (route: string) => this.router.url == route
}
