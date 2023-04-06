import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-navbar-dropdown-item',
  templateUrl: './navbar-dropdown-item.component.html',
  styleUrls: ['./navbar-dropdown-item.component.css']
})
export class NavbarDropdownItemComponent {
  @Input() link = ''
  @Input() name = ''
  @Output() click: EventEmitter<any> = new EventEmitter()
  @Input() isActive: boolean = false

  onClick = () => this.click.emit()
}
