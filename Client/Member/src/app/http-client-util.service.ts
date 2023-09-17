import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable, map, tap, of } from 'rxjs';
import { AppSettingsService } from './app-settings.service';
import { TokenService } from './services/token.service';

@Injectable({
  providedIn: 'root'
})
export class HttpClientUtilService {

  constructor(private appSettings: AppSettingsService,
    private oidcSecurityService: OidcSecurityService) { }

  GetApiBaseAddress() : string {
    return this.appSettings.GetSettings().ApiBaseAddress;
  }

  GetLoanApiBaseAddress() : string {
    return this.appSettings.GetSettings().LoanApiBaseAddress;
  }

  CreateUserTokenAuthHeader() : Observable<HttpHeaders> {
    return this.oidcSecurityService.getIdToken()
    .pipe(
      map(tkn => this.CreateAuthorizationHeader(tkn))
    );
  }

  CreateAuthHeader(tokenService: TokenService) : Observable<HttpHeaders> {
    return this.GetToken(tokenService)
    .pipe(
      map(tkn => this.CreateAuthorizationHeader(tkn))      
      );
  }

  GetToken(tokenService: TokenService) : Observable<string> {
    if (this.IsCachedTokenAvailable()) {
      return of(this.GetCachedToken() || "");
    }
    else {
      return tokenService.GetToken()
      .pipe(
        tap(tkn => this.TapAccessToken(tkn))
      );
    }
  }

  private TapAccessToken(tkn: string): void {
    sessionStorage.setItem("AccessToken", tkn);
    let expiration: Date = new Date();
    expiration.setMinutes(expiration.getMinutes() + 59);
    sessionStorage.setItem("AccessToknExpiration", expiration.valueOf().toString());
  }

  /*GetRoles(tokenService: TokenService) : Promise<Array<string>> {
    return this.GetToken(tokenService)
    .then(tkn => {
      const decoded : any = jwt_decode(tkn);
      if (decoded && decoded.role && Array.isArray(decoded.role)) 
      {
        return decoded.role;
      }
      else if (decoded && decoded.role) {
        return [ decoded.role ];
      }
      else {
        return [];
      }      
    });
  }*/

  private CreateAuthorizationHeader(tkn: string) : HttpHeaders {
    return new HttpHeaders({"Authorization": `bearer ${tkn}`})
  }

  private IsCachedTokenAvailable() : boolean {
    const tkn = sessionStorage.getItem("AccessToken");
    const expiration = sessionStorage.getItem("AccessToknExpiration");
    let result: boolean = false;
    if (tkn && expiration) {
      if (Date.now() < Number(expiration)) {
        result = true;
      }
      let dt: Date = new Date(Number(expiration));
    }
    return result;
  }

  private GetCachedToken() : string | null {
    return sessionStorage.getItem("AccessToken");
  }

  DropCache() : void {
    sessionStorage.removeItem("AccessToken");
    sessionStorage.removeItem("AccessToknExpiration");
  }
}
