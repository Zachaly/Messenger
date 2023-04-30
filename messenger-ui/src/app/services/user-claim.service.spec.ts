import { TestBed } from '@angular/core/testing';

import { UserClaimService } from './user-claim.service';

describe('UserClaimService', () => {
  let service: UserClaimService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserClaimService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
