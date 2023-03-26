import { Component, EventEmitter, Input, Output } from '@angular/core';
import NotificationModel from 'src/app/models/NotificationModel';
import { faXmark, faCheck } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent {

  @Input() notification: NotificationModel = { text: '', read: false }
  @Output() delete: EventEmitter<any> = new EventEmitter()
  xMark = faXmark
  check = faCheck
  
  onRead() {
    this.notification.read = true
  }

  onDelete() {
    this.delete.emit()
  }
} 
