import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent {
  @Input() text: string = ''
  @Output() read: EventEmitter<any> = new EventEmitter()

  onRead() {
    this.read.emit()
  }
} 
