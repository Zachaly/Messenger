import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMessageReportComponent } from './add-message-report.component';

describe('AddMessageReportComponent', () => {
  let component: AddMessageReportComponent;
  let fixture: ComponentFixture<AddMessageReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddMessageReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddMessageReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
