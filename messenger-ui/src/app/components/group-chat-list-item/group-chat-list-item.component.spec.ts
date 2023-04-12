import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupChatListItemComponent } from './group-chat-list-item.component';

describe('GroupChatListItemComponent', () => {
  let component: GroupChatListItemComponent;
  let fixture: ComponentFixture<GroupChatListItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupChatListItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GroupChatListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
