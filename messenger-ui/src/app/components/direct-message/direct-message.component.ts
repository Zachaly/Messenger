import { Component, EventEmitter, Input, Output } from '@angular/core';
import DirectMessage from 'src/app/models/DirectMessage';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-direct-message',
  templateUrl: './direct-message.component.html',
  styleUrls: ['./direct-message.component.css']
})
export class DirectMessageComponent {
  @Input() message: DirectMessage = { id: 0, content: '', read: false, created: '', senderName: '', senderId: 0, imageIds: [] }
  @Output() selectEmoji: EventEmitter<string> = new EventEmitter()
  showEmoji = false
  report = false
  
  constructor(private imageService: ImageService) {

  }

  formatDate() {
    return this.message.created.replace('T', ' ')
      .substring(0, this.message.created.lastIndexOf('.'))
  }

  getImage = (id: number) => this.imageService.getUrl('direct-message', id)

  onSelectEmoji(emoji: string) {
    this.showEmoji = false
    this.selectEmoji.emit(emoji)
  }
}
