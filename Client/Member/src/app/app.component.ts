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
          if (!isAuthenticated.isAuthenticated) {
              this.UserImageSource = null;
              //this.httpClientUtilService.DropCache();
              this.Login();
          }
          if (isAuthenticated.isAuthenticated) {
              this.navigateToStoredEndpoint();
          }
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

  private navigateToStoredEndpoint() {
      const path: string = this.read('redirect');
      const href: any = this.read('redirect-href');
      this.write("redirect", null);
      this.write("redirect-href", null);
      if (path && path != "")
      {
          if (this.router.url === path) {
              return;
          }
  
          if (path.toString().includes('/unauthorized')) {
              this.router.navigate(['/']);
          } 
          else if (href && href != '') {
            console.log(`navigate to url ${href}`);
            window.location = href;
          }
          else {
              this.router.navigate([path]);
          }
      }
  }

  private read(key: string): any {
      const data = localStorage.getItem(key);
      if (data) {
          return JSON.parse(data);
      }

      return;
  }

  private write(key: string, value: any): void {
      localStorage.setItem(key, JSON.stringify(value));
  }

  Login() {
    if ('/autologin' !== window.location.pathname) {
      this.write("redirect", window.location.pathname);
      this.write("redirect-href", window.location.href);
      this.router.navigate(['/autologin']);
    }
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
