import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectChatListItemComponent } from './direct-chat-list-item.component';

describe('DirectChatListItemComponent', () => {
  let component: DirectChatListItemComponent;
  let fixture: ComponentFixture<DirectChatListItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DirectChatListItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DirectChatListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
