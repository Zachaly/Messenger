import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ChatMessageModel from 'src/app/models/ChatMessageModel';
import DirectMessage from 'src/app/models/DirectMessage';
import MessageReport from 'src/app/models/MessageReport';
import MessageType from 'src/app/models/enum/MessageType';
import { ChatMessageService } from 'src/app/services/chat-message.service';
import { DirectMessageService } from 'src/app/services/direct-message.service';
import { MessageReportService } from 'src/app/services/message-report.service';
import { UserBanService } from 'src/app/services/user-ban.service';
import { UserClaimService } from 'src/app/services/user-claim.service';

@Component({
  selector: 'app-message-report-page',
  templateUrl: './message-report-page.component.html',
  styleUrls: ['./message-report-page.component.css']
})
export class MessageReportPageComponent implements OnInit {

  report: MessageReport = {
    id: 0,
    reportedUserId: 0,
    reportingUserName: '',
    reportDate: '',
    reportingUserId: 0,
    attachedMessageId: 0,
    reason: '',
    messageType: 1,
    resolved: false
  }

  message!: DirectMessage | ChatMessageModel

  banStart: Date = new Date()
  banEnd: Date = new Date()

  constructor(private route: ActivatedRoute, private messageReportService: MessageReportService,
    private chatMessageService: ChatMessageService, private directMessageService: DirectMessageService,
    private userBanService: UserBanService, private router: Router, private userClaimService: UserClaimService) {

  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.messageReportService.getMessageReport({ id: params['id'] }).subscribe(res => {
        this.report = res[0]!

        if (this.report.messageType == MessageType.Direct) {
          this.directMessageService.getMessages({ id: this.report.attachedMessageId }).subscribe(res => {
            this.message = res[0]
          })
        } else if (this.report.messageType == MessageType.Group) {
          this.chatMessageService.getChatMessage({ id: this.report.attachedMessageId }).subscribe(res => {
            this.message = res[0]
          })
        }
      })
    })
  }

  accept() {
    this.messageReportService.updateMessageReport({ id: this.report.id, resolved: true }).subscribe(() => {
      this.userBanService.postUserBan({
        userId: this.report.reportedUserId,
        start: this.banStart,
        end: this.banEnd
      }).subscribe(() => {
        this.userClaimService.addUserClaim({ userId: this.report.reportedUserId, value: 'Ban' }).subscribe(() => {
          this.router.navigateByUrl('/moderation')
        })
      })
    })
  }

  deny() {
    this.messageReportService.updateMessageReport({ id: this.report.id, resolved: true }).subscribe(() => {
      this.router.navigateByUrl('/moderation')
    })
  }
}
