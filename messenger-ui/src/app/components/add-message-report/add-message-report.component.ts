import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import ChatMessageModel from 'src/app/models/ChatMessageModel';
import DirectMessage from 'src/app/models/DirectMessage';
import MessageType from 'src/app/models/enum/MessageType';
import AddMessageReportRequest from 'src/app/requests/AddMessageReportRequest';
import { AuthService } from 'src/app/services/auth.service';
import { MessageReportService } from 'src/app/services/message-report.service';

@Component({
  selector: 'app-add-message-report',
  templateUrl: './add-message-report.component.html',
  styleUrls: ['./add-message-report.component.css']
})
export class AddMessageReportComponent implements OnInit {
  @Input() message!: DirectMessage | ChatMessageModel
  @Output() close: EventEmitter<any> = new EventEmitter()

  report: AddMessageReportRequest = { userId: 0, reason: '', reportedUserId: 0, messageId: 0, messageType: MessageType.Direct }

  constructor(private authService: AuthService, private messageReportService: MessageReportService){

  }
  ngOnInit(): void {
    this.report = {
      userId: this.authService.currentUser.userId,
      reportedUserId: this.message.senderId,
      reason: '',
      messageType: (this.message as ChatMessageModel).readByIds ? MessageType.Group : MessageType.Direct,
      messageId: this.message.id
    }
  }

  send() {
    this.messageReportService.postMessageReport(this.report).subscribe(() => {
      this.close.emit()
    })
  }
}
