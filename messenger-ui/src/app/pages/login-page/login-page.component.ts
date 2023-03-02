import { Component } from '@angular/core';
import { Router } from '@angular/router';
import LoginRequest from 'src/app/requests/LoginRequest';
import { AuthService } from 'src/app/services/auth.service';

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

  constructor(private authService: AuthService, private router: Router) { }

  submit(){
    this.authService.login(this.loginRequest, () => this.router.navigateByUrl('/'))
  }
}
