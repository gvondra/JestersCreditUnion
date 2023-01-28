import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styles: []
})
export class AppComponent {
  title = 'Member';
  UserImageSource: string | null = null;
  IsLoggedIn: boolean = false;

  constructor(public oidcSecurityService: OidcSecurityService,
    private router: Router) {}

    ngOnInit(): void {
      console.log("Start app component init")
      this.IsLoggedIn = false;
      this.UserImageSource = null;
      this.oidcSecurityService
      .checkAuth()
      .subscribe((isAuthenticated) => {
          console.log(`app checkAuth ${isAuthenticated.isAuthenticated}`)
          /*if (!isAuthenticated.isAuthenticated) {
              this.UserImageSource = null;
              this.httpClientUtilService.DropCache();
              if (!window.location.pathname.endsWith('autologin')) {
                  this.write('redirect', window.location.pathname);
                  this.router.navigate(['/autologin']);
              }
          }
          if (isAuthenticated.isAuthenticated) {
              this.navigateToStoredEndpoint();
          }*/ 
      });
      this.oidcSecurityService.isAuthenticated$.subscribe(isAuthenticated => {
        console.log(`app authenticated ${isAuthenticated.isAuthenticated}`)
        this.IsLoggedIn = isAuthenticated.isAuthenticated;
        if (isAuthenticated.isAuthenticated) {
            this.GetUserImageSource();
        }
        else {
            this.UserImageSource = null;
        }
      });
  }    

  NavigateHome()
  {
      this.router.navigate(["/"]);
  }
  
  Login() {
    this.oidcSecurityService.authorize();
  }

  private GetUserImageSource() {
      this.oidcSecurityService.getPayloadFromIdToken().subscribe(tkn => {
        if (tkn && tkn.picture) {
          this.UserImageSource = tkn.picture;
        }
        else {
          this.UserImageSource = null;
        }
      });      
      
  }

}
