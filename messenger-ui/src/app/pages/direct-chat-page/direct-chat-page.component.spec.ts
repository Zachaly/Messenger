import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectChatPageComponent } from './direct-chat-page.component';

describe('DirectChatPageComponent', () => {
  let component: DirectChatPageComponent;
  let fixture: ComponentFixture<DirectChatPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DirectChatPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DirectChatPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
