import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import LoginRequest from 'src/app/requests/LoginRequest';
import { AuthService } from 'src/app/services/auth.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {
  loginRequest: LoginRequest = {
    login: '',
    password: ''
  }

  rememberMe: boolean = false

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.onAuthChange().subscribe(res => {
      if (res.authToken) {
        if (this.rememberMe) {
          this.authService.saveUserData()
        }
        this.router.navigateByUrl('/')
      }
    })
    this.authService.loadUserData()
  }

  submit() {
    this.authService.login(this.loginRequest, err => alert(err.error.error))
  }
}
