import { Component } from '@angular/core';
import { Router } from '@angular/router';
import AddUserRequest from 'src/app/requests/AddUserRequest';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent {
  registerRequest: AddUserRequest = {
    login: '',
    name: '',
    password: ''
  }

  errors: { Password?: string[], Name?: string[], Login?: string[] } = {}

  constructor(private authService: AuthService, private router: Router) { }

  submit() {
    this.authService.register(this.registerRequest).subscribe({
      next: res => this.router.navigateByUrl('/login'),
      error: err => {
        if (err.error.errors) {
          this.errors = err.error.errors
        }

        if (err.error.error) {
          alert(err.error.error)
        }
      }
    })
  }
}
