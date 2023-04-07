import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-emoji-select',
  templateUrl: './emoji-select.component.html',
  styleUrls: ['./emoji-select.component.css']
})
export class EmojiSelectComponent {
  emojis: string[] = [
    '🙂', '😀', '😃', '😄', '😁', '😅', '🤣', '😇', '😎', '🥰', '😘', '😍',
    '😛', '😜', '😐', '🤢', '🥵', '🥶', '😕', '🙁', '😳', '🥺', '😥', '😭',
    '🤬', '💗', '👌', '👆', '🖕', '👍', '👎', '🙏'
  ]
  @Output() select: EventEmitter<string> = new EventEmitter()

  selectEmoji(emoji: string) {
    this.select.emit(emoji)
  }
}
