import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RequestMembershipComponent } from './request-membership/request-membership.component';
import { AuthGuard } from './auth/auth.guard';
const routes: Routes = [
    {
        path: '',
        component: HomeComponent
      },
      {
        path: 'join',
        component: RequestMembershipComponent,
        canActivate: [AuthGuard]
      }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class AppRoutingModule { }