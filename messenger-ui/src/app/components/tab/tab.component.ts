import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-tab',
  templateUrl: './tab.component.html',
  styleUrls: ['./tab.component.css']
})
export class TabComponent {
  @Input() names: string[] = []
  @Output() select: EventEmitter<number> = new EventEmitter()
  selectedIndex = 0

  onSelect(index: number) {
    this.selectedIndex = index
    this.select.emit(index)
  }
}
