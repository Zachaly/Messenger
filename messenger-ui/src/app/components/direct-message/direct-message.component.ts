import { Component, Input } from '@angular/core';
import DirectMessage from 'src/app/models/DirectMessage';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-direct-message',
  templateUrl: './direct-message.component.html',
  styleUrls: ['./direct-message.component.css']
})
export class DirectMessageComponent {
  @Input() message: DirectMessage = { id: 0, content: '', read: false, created: '', senderName: '', senderId: 0, imageIds: [] }

  constructor(private imageService: ImageService) {

  }

  formatDate() {
    return this.message.created.replace('T', ' ')
      .substring(0, this.message.created.lastIndexOf('.'))
  }

  getImage = (id: number) => this.imageService.getUrl('direct-message', id)
}
