import { Component, EventEmitter, Input, Output } from '@angular/core';
import AddChatRequest from 'src/app/requests/AddChatRequest';
import { AuthService } from 'src/app/services/auth.service';
import { ChatService } from 'src/app/services/chat.service';

@Component({
  selector: 'app-add-chat',
  templateUrl: './add-chat.component.html',
  styleUrls: ['./add-chat.component.css']
})
export class AddChatComponent {
  request: AddChatRequest = { name: '', userId: 0 }

  @Output() submit: EventEmitter<any> = new EventEmitter()
  @Output() cancel: EventEmitter<any> = new EventEmitter()

  errors: { Name?: string[] } = {}

  constructor(authService: AuthService, private chatService: ChatService) {
    this.request.userId = authService.currentUser.userId
  }

  onSubmit() {
    this.chatService.addChat(this.request).subscribe({
      next: res => {
        alert('Chat added!')
        this.submit.emit()
      },
      error: err => {
        if (err.error.errors) {
          this.errors = err.error.errors
        }
      }
    })
  }

  onCancel() {
    this.cancel.emit()
  }
}
