import { TestBed } from '@angular/core/testing';

import { ChatMessageReactionService } from './chat-message-reaction.service';

describe('ChatMessageReactionService', () => {
  let service: ChatMessageReactionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatMessageReactionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
