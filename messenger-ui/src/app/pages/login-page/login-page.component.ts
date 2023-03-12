import { Component } from '@angular/core';
import { Router } from '@angular/router';
import LoginRequest from 'src/app/requests/LoginRequest';
import { AuthService } from 'src/app/services/auth.service';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  loginRequest: LoginRequest = {
    login: '',
    password: ''
  }

  constructor(private authService: AuthService, private router: Router, private signalR: SignalrService) { }

  submit(){
    this.authService.login(this.loginRequest, () => this.router.navigateByUrl('/'))
  }
}
