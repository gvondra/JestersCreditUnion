import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { debounce, interval, map, Observable, take, tap } from 'rxjs';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard  {
  
  constructor (
    private oidcSecurityService: OidcSecurityService
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      return this.oidcSecurityService.isAuthenticated$.pipe(
        //tap(isAuthenticated => console.log(`in auth guard; isAuthenticated = ${isAuthenticated.isAuthenticated}`)),
        take(12),
        debounce(isAuthenticated => {
          if (!isAuthenticated.isAuthenticated) {
            return interval(200);
          }          
          else {
            return interval(0);
          }
        }),
        map(isAuthenticated => isAuthenticated.isAuthenticated)
      );
  }
  
}
