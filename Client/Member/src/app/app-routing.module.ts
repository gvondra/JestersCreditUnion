import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { HomeComponent } from './home/home.component';
import { LoanApplicationComponent } from './loan-application/loan-application.component';
import { LoanApplicationsComponent } from './loan-applications/loan-applications.component';
import { AutoLoginComponent } from './auto-login/auto-login.component';

const routes: Routes = [
  {
    path: "autologin",
    component: AutoLoginComponent
  },
  {
    path: "",
    component: HomeComponent
  },
  {
    path: "home",
    component: HomeComponent
  },
  {
    path: "LoanApplication",
    component: LoanApplicationComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "LoanApplications",
    component: LoanApplicationsComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
