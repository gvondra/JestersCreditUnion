import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router'
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { HeaderComponent } from './header/header.component';
import { RequestMembershipComponent } from './request-membership/request-membership.component';
import { MenuComponent } from './menu/menu.component';
import { ConfigurationService } from './configuration.service';
import { AppRoutingModule } from './app-routing.module';
import { AuthService } from './auth/auth.service';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeComponent,
    RequestMembershipComponent,
    MenuComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    AppRoutingModule
  ],
  providers: [ConfigurationService, AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
