import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { HomeComponent } from './home/home.component';
import { LoanApplicationComponent } from './loan-application/loan-application.component';

const routes: Routes = [
  {
    path: "",
    component: HomeComponent
  },
  {
    path: "LoanApplication",
    component: LoanApplicationComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
