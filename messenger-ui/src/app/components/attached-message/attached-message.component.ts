import { Component, Input } from '@angular/core';
import ChatMessageModel from 'src/app/models/ChatMessageModel';
import DirectMessage from 'src/app/models/DirectMessage';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-attached-message',
  templateUrl: './attached-message.component.html',
  styleUrls: ['./attached-message.component.css']
})
export class AttachedMessageComponent {
  @Input() message!: DirectMessage | ChatMessageModel

  constructor(private imageService: ImageService){

  }

  getImage(id: number): string {
    if((this.message as ChatMessageModel).readByIds){
      return this.imageService.getUrl('chat-message', id)
    }

    return this.imageService.getUrl('direct-message', id)
  }
}
