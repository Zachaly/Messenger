import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddChatUserComponent } from './add-chat-user.component';

describe('AddChatUserComponent', () => {
  let component: AddChatUserComponent;
  let fixture: ComponentFixture<AddChatUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddChatUserComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddChatUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
