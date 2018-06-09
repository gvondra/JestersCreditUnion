import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './auth/auth.guard';
import { CallbackComponent } from './callback/callback.component';
import { RoleRequestComponent } from './role-request/role-request.component';
import { GroupsComponent } from './groups/groups.component';
import { GroupComponent } from './group/group.component';
import { UserSearchComponent } from './user-search/user-search.component';
import { UserComponent } from './user/user.component';
import { UserGroupsComponent } from './user-groups/user-groups.component';
import { WebMetricsComponent } from './web-metrics/web-metrics.component';
import { EventTypeListComponent } from './event-type-list/event-type-list.component';
import { EventTypeComponent } from './event-type/event-type.component';
const routes: Routes = [
    {
        path: '',
        component: HomeComponent
      },
      {
        path: 'callback',
        component: CallbackComponent
      },
      {
        path: 'rolerequest',
        component: RoleRequestComponent,
        canActivate: [
          AuthGuard
        ]
      },
      {
        path: 'grouplist',
        component: GroupsComponent,
        canActivate: [
          AuthGuard
        ]
      },
      {
        path: 'group',
        component: GroupComponent,
        canActivate: [
          AuthGuard
        ]
      },
      {
        path: 'group/:id',
        component: GroupComponent,
        canActivate: [
          AuthGuard
        ]
      },
      {
        path: 'usersearch',
        component: UserSearchComponent,
        canActivate: [
          AuthGuard
        ]
      },
      {
        path: 'user/:id',
        component: UserComponent,
        canActivate: [
          AuthGuard
        ]
      },
      {
        path: 'user/:id/groups',
        component: UserGroupsComponent,
        canActivate: [
          AuthGuard
        ]
      },
      {
        path: 'webmetrics',
        component: WebMetricsComponent,
        canActivate: [
          AuthGuard
        ]
      },
      {
        path: 'eventtypelist',
        component: EventTypeListComponent,
        canActivate: [
          AuthGuard
        ]
      },
      {
        path: 'eventtype',
        component: EventTypeComponent,
        canActivate: [
          AuthGuard
        ]
      },
      {
        path: 'eventtype/:id',
        component: EventTypeComponent,
        canActivate: [
          AuthGuard
        ]
      }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class AppRoutingModule { }