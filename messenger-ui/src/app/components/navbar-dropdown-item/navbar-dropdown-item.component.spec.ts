import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarDropdownItemComponent } from './navbar-dropdown-item.component';

describe('NavbarDropdownItemComponent', () => {
  let component: NavbarDropdownItemComponent;
  let fixture: ComponentFixture<NavbarDropdownItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NavbarDropdownItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarDropdownItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
