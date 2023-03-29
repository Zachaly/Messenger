import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OnlineFriendsBarComponent } from './online-friends-bar.component';

describe('OnlineFriendsBarComponent', () => {
  let component: OnlineFriendsBarComponent;
  let fixture: ComponentFixture<OnlineFriendsBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OnlineFriendsBarComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OnlineFriendsBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
