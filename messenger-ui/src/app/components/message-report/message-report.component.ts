import { Component, Input } from '@angular/core';
import MessageReport from 'src/app/models/MessageReport';

@Component({
  selector: 'app-message-report',
  templateUrl: './message-report.component.html',
  styleUrls: ['./message-report.component.css']
})
export class MessageReportComponent {
  @Input() report!: MessageReport
}
