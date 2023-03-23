import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import NotificationModel from '../models/NotificationModel';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notifications: NotificationModel[] = []
  notificationCountSubject: Subject<number> = new Subject()
  notificationSubject: Subject<NotificationModel[]> = new Subject()
  constructor() { }

  addNotification(text: string) {
    this.notifications.push({ text, read: false })
    this.notificationCountSubject.next(this.notifications.filter(x => !x.read).length)
    this.notificationSubject.next(this.notifications)
  }
}
