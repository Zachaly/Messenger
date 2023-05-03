import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttachedMessageComponent } from './attached-message.component';

describe('AttachedMessageComponent', () => {
  let component: AttachedMessageComponent;
  let fixture: ComponentFixture<AttachedMessageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AttachedMessageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AttachedMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
