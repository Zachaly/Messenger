import { TestBed } from '@angular/core/testing';

import { ChatMessageReadService } from './chat-message-read.service';

describe('ChatMessageReadService', () => {
  let service: ChatMessageReadService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatMessageReadService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
