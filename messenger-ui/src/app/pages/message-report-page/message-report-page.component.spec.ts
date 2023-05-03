import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageReportPageComponent } from './message-report-page.component';

describe('MessageReportPageComponent', () => {
  let component: MessageReportPageComponent;
  let fixture: ComponentFixture<MessageReportPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MessageReportPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MessageReportPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
