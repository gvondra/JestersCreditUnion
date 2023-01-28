import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AuthModule, LogLevel, OidcSecurityService, StsConfigHttpLoader, StsConfigLoader } from 'angular-auth-oidc-client';
import { from } from 'rxjs';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { AppSettingsService } from './app-settings.service';

export const httpLoaderFactory = (appSettingsService: AppSettingsService) => {  
  const settings$: Promise<any> = appSettingsService.LoadSettings()
  .then((settings) => 
  {    
    return {
      authority: settings.GoogleStsServer,     
      authWellknownEndpointUrl: settings.AuthWellknownEndpointUrl,
      redirectUrl: window.location.origin + settings.BaseHref + "/",
      clientId: settings.GoogleClientId,
      responseType: 'id_token token',
      scope: 'openid email profile',
      triggerAuthorizationResultEvent: true,
      postLogoutRedirectUri: window.location.origin + '/unauthorized',
      startCheckSession: false,
      silentRenew: false,
      silentRenewUrl: window.location.origin + '/silent-renew.html',
      postLoginRoute: '/home',
      forbiddenRoute: '/forbidden',
      unauthorizedRoute: '/unauthorized',
      logLevel: LogLevel.Debug,
      historyCleanupOff: true,
      autoUserinfo: false
      // iss_validation_off: false
      // disable_iat_offset_validation: true
    }
  });

  return new StsConfigHttpLoader(from(settings$));
};

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    AuthModule.forRoot({
      loader: {
        provide: StsConfigLoader,
        useFactory: httpLoaderFactory,
        deps: [AppSettingsService],
      }
    })    
  ],
  providers: [
    AppSettingsService,
    OidcSecurityService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
