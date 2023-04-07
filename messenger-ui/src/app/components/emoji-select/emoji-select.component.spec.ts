import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmojiSelectComponent } from './emoji-select.component';

describe('EmojiSelectComponent', () => {
  let component: EmojiSelectComponent;
  let fixture: ComponentFixture<EmojiSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmojiSelectComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmojiSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
