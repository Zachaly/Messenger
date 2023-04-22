import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatMessageReactionComponent } from './chat-message-reaction.component';

describe('ChatMessageReactionComponent', () => {
  let component: ChatMessageReactionComponent;
  let fixture: ComponentFixture<ChatMessageReactionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChatMessageReactionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChatMessageReactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
