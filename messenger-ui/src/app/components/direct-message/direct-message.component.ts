import { Component, Input } from '@angular/core';
import DirectMessage from 'src/app/models/DirectMessage';

@Component({
  selector: 'app-direct-message',
  templateUrl: './direct-message.component.html',
  styleUrls: ['./direct-message.component.css']
})
export class DirectMessageComponent {
  @Input() message: DirectMessage = { id: 0, content: '', read: false, created: '', senderName: '', senderId: 0 }

  formatDate() {
    return this.message.created.replace('T', ' ')
      .substring(0, this.message.created.lastIndexOf('.'))
  }
}
