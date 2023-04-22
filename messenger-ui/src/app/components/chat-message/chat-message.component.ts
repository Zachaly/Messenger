import { Component, EventEmitter, Input, Output } from '@angular/core';
import ChatMessageModel from 'src/app/models/ChatMessageModel';
import ChatMessageReaction from 'src/app/models/ChatMessageReaction';
import { ImageService } from 'src/app/services/image.service';

@Component({
  selector: 'app-chat-message',
  templateUrl: './chat-message.component.html',
  styleUrls: ['./chat-message.component.css']
})
export class ChatMessageComponent {
  @Input() message: ChatMessageModel = { id: 0, content: '', senderId: 0, senderName: '', readByIds: [], imageIds: [], reactions: [] }
  @Output() selectEmoji: EventEmitter<string> = new EventEmitter()
  @Output() clickReaction: EventEmitter<ChatMessageReaction> = new EventEmitter()
  showEmoji = false

  constructor(private imageService: ImageService) { }

  getProfileUrl = (id: number) : string => this.imageService.getUrl('profile', id)

  getImageUrl = (id: number): string => this.imageService.getUrl('chat-message', id)

  onSelectEmoji(emoji: string){
    this.selectEmoji.emit(emoji)
    this.showEmoji = false
  }

  onReactionClick(reaction: ChatMessageReaction) {
    this.clickReaction.emit(reaction)
  }
}
