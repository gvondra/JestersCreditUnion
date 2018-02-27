import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  hidden : boolean = true;
  @Input("auth") authService: AuthService;
  @Input() title: string;
  constructor() { }

  toggleVissible() {
    this.hidden = !this.hidden;
  }

  ngOnInit() {
    
  }

}
