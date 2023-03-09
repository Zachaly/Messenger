import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendListPageComponent } from './friend-list-page.component';

describe('FriendListPageComponent', () => {
  let component: FriendListPageComponent;
  let fixture: ComponentFixture<FriendListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FriendListPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FriendListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
