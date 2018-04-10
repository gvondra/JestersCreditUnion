import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { MenuItem } from '../menu-item';
import { MenuService } from '../menu.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  providers: [MenuService]
})
export class HeaderComponent implements OnInit {

  hidden : boolean = true;
  sections : Array<Array<MenuItem>>;
  errorMessage : string = null;
  @Input("auth") authService: AuthService;
  @Input() title: string;
  constructor(private menuService : MenuService) { }

  showMenu(): void {
    this.menuService.getMenuItems().then(items => {this.sections = items; this.hidden = false; this.errorMessage = null;})
    .catch(er => {this.errorMessage = "Failed to load menu";});
  }

  toggleVissible() {
    if (this.hidden) {
      this.showMenu();
    }
    else {
      this.hidden = true;
    }   
  }

  ngOnInit() {
    
  }

}
