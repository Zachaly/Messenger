import { Component, EventEmitter, Input, Output } from '@angular/core';
import ChatMessageReaction from 'src/app/models/ChatMessageReaction';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-chat-message-reaction',
  templateUrl: './chat-message-reaction.component.html',
  styleUrls: ['./chat-message-reaction.component.css']
})
export class ChatMessageReactionComponent {
  @Input() reaction: ChatMessageReaction = { userId: 0, reaction: '' }
  @Output() click: EventEmitter<ChatMessageReaction> = new EventEmitter()

  hover = false

  constructor(private imageService: ImageService) {}

  onClick() : boolean {
    this.click.emit(this.reaction)
    return false
  }

  userImage = (): string => this.imageService.getUrl('profile', this.reaction.userId)

  changeHover(hover: boolean){
    this.hover = hover
  }
}
