import { TestBed } from '@angular/core/testing';

import { UserBanService } from './user-ban.service';

describe('UserBanService', () => {
  let service: UserBanService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserBanService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
