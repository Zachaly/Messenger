import { Component, ElementRef, ViewChild } from '@angular/core';
import UserListItem from 'src/app/models/UserListItem';
import ChangeUserPasswordRequest from 'src/app/requests/ChangeUserPasswordRequest';
import UpdateUsernameRequest from 'src/app/requests/UpdateUsernameRequest';
import { AuthService } from 'src/app/services/auth.service';
import { ImageService } from 'src/app/services/image.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-update-profile-page',
  templateUrl: './update-profile-page.component.html',
  styleUrls: ['./update-profile-page.component.css']
})
export class UpdateProfilePageComponent {

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>

  selectedFile: File | null = null

  user: UserListItem = { id: 0, name: '' }
  passwordRequest: ChangeUserPasswordRequest = { userId: 0, currentPassword: '', newPassword: '' }

  nameErrors: { Name?: string[] } = {}
  passwordErrors: { NewPassword?: string[] } = {}

  constructor(private imageService: ImageService, private authService: AuthService, private userService: UserService) {
    this.passwordRequest.userId = authService.currentUser.userId
    userService.getUsers({ id: authService.currentUser.userId }).subscribe(res => {
      this.user = res[0]!
    })
  }

  submitImage() {
    this.imageService.uploadProfileImage(this.authService.currentUser.userId, this.selectedFile).subscribe({
      next: () => alert('Profile image updated!'),
      error: err => alert('Could not update image!')
    })
  }

  submitName() {
    const request: UpdateUsernameRequest = {
      id: this.authService.currentUser.userId,
      name: this.user.name
    }
    this.userService.updateUser(request).subscribe({
      next: () => alert('Name updated'),
      error: err => {
        if (err.error.errors) {
          this.nameErrors = err.error.errors
        }

        if (err.error.error) {
          alert(err.error.error)
        }
      }
    })
  }

  changeImage() {
    this.selectedFile = this.fileInput.nativeElement.files![0]
    console.log(this.selectedFile)
  }

  changePassword() {
    this.userService.changePassword(this.passwordRequest).subscribe({
      next: () => {
        alert("Password updated")
      },
      error: (err) => {
        console.log(err)
        if (err.error.errors) {
          this.passwordErrors = err.error.errors
        }

        if (err.error.error) {
          alert(err.error.error)
        }
      }
    })
  }
}
