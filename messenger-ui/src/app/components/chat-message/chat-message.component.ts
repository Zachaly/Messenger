import { Component, Input } from '@angular/core';
import ChatMessageModel from 'src/app/models/ChatMessageModel';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-chat-message',
  templateUrl: './chat-message.component.html',
  styleUrls: ['./chat-message.component.css']
})
export class ChatMessageComponent {
  @Input() message: ChatMessageModel = { id: 0, content: '', senderId: 0, senderName: '', readByIds: [], imageIds: [] }

  constructor(private imageService: ImageService) { }

  getProfileUrl = (id: number) : string => this.imageService.getUrl('profile', id)

  getImageUrl = (id: number): string => this.imageService.getUrl('chat-message', id)
}
