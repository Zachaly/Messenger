import { Component, OnInit } from '@angular/core';
import MessageReport from 'src/app/models/MessageReport';
import UserBan from 'src/app/models/UserBan';
import { MessageReportService } from 'src/app/services/message-report.service';
import { UserBanService } from 'src/app/services/user-ban.service';
import { UserClaimService } from 'src/app/services/user-claim.service';

@Component({
  selector: 'app-moderation-page',
  templateUrl: './moderation-page.component.html',
  styleUrls: ['./moderation-page.component.css']
})
export class ModerationPageComponent implements OnInit {

  tabs = ['reports', 'bans']
  tabIndex = 0

  reports: MessageReport[] = []
  bans: UserBan[] = []

  constructor(private messageReportService: MessageReportService, private userBanService: UserBanService, 
    private userClaimService: UserClaimService) {
  }

  ngOnInit(): void {
    this.changeTab(0)
  }
  
  changeTab(index: number){
    this.tabIndex = index

    if(index == 0){
      this.messageReportService.getMessageReport({ resolved: false }).subscribe(res => {
        this.reports = res
      })
    }

    if(index == 1){
      this.userBanService.getUserBan({ }).subscribe(res => {
        this.bans = res
      })
    }
  }

  liftBan(ban: UserBan) {
    this.userBanService.deleteUserBan(ban.id).subscribe(() => {
      this.userClaimService.deleteUserClaim(ban.userId, 'Ban').subscribe(() => {
        this.bans = this.bans.filter(x => x.id !== ban.id)
      })
    })
  }
}
