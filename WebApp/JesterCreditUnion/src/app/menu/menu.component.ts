import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';
import { HeaderComponent } from '../header/header.component';
import { MenuItem } from '../menu-item';
import { MenuService } from '../menu.service';
@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
  providers: [MenuService]
})
export class MenuComponent implements OnInit {

  sections : Array<Array<MenuItem>>;
  @Input() headerComponent : HeaderComponent;
  @Output() onClick = new EventEmitter();

  constructor(private menuService : MenuService) { }

  getMenuItems(): void {
    this.menuService.getMenuItems().then(items => this.sections = items);
  }

  toggleVissible() {
    this.onClick.emit();
  }

  ngOnInit() {
    this.getMenuItems();
  }

}
