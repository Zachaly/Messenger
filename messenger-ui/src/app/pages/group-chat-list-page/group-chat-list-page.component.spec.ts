import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupChatListPageComponent } from './group-chat-list-page.component';

describe('GroupChatListPageComponent', () => {
  let component: GroupChatListPageComponent;
  let fixture: ComponentFixture<GroupChatListPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupChatListPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GroupChatListPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
