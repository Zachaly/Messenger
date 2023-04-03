import { Component, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-update-profile-page',
  templateUrl: './update-profile-page.component.html',
  styleUrls: ['./update-profile-page.component.css']
})
export class UpdateProfilePageComponent {

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>

  selectedFile: File | null = null

  constructor(private imageService: ImageService, private authService: AuthService) {

  }

  submitImage() {
    this.imageService.uploadProfileImage(this.authService.currentUser.userId, this.selectedFile).subscribe(() => alert('Profile image updated!'))
  }

  changeImage() {
    this.selectedFile = this.fileInput.nativeElement.files![0]
    console.log(this.selectedFile)
  }
}
