import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendRequestListPageComponent } from './friend-request-list-page.component';

describe('FriendRequestListPageComponent', () => {
  let component: FriendRequestListPageComponent;
  let fixture: ComponentFixture<FriendRequestListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FriendRequestListPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FriendRequestListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
