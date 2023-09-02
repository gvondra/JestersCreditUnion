import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: [
  ]
})
export class HomeComponent implements OnInit {

  IsAuthenticated: boolean = false;

  constructor(public oidcSecurityService: OidcSecurityService) { }

  ngOnInit(): void {
    this.oidcSecurityService.isAuthenticated$.subscribe(authResult => {
      this.IsAuthenticated = authResult.isAuthenticated;
    });
  }

}
