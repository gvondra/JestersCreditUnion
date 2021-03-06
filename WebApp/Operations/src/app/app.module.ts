import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { HeaderComponent } from './header/header.component';
import { AppRoutingModule } from './app-routing.module';
import { AuthService } from './auth/auth.service';
import { CallbackComponent } from './callback/callback.component';
import { RoleRequestComponent } from './role-request/role-request.component';
import { GroupsComponent } from './groups/groups.component';
import { GroupComponent } from './group/group.component';
import { UserSearchComponent } from './user-search/user-search.component';
import { UserComponent } from './user/user.component';
import { UserGroupsComponent } from './user-groups/user-groups.component';
import { WebMetricsComponent } from './web-metrics/web-metrics.component';
import { EventTypeComponent } from './event-type/event-type.component';
import { EventTypeListComponent } from './event-type-list/event-type-list.component';
import { TaskTypeListComponent } from './task-type-list/task-type-list.component';
import { TaskTypeGroupsComponent } from './task-type-groups/task-type-groups.component';
import { TaskTypeEventTypesComponent } from './task-type-event-types/task-type-event-types.component';
import { TaskTypeComponent } from './task-type/task-type.component';
import { MyTasksComponent } from './my-tasks/my-tasks.component';
import { UnassignedTasksComponent } from './unassigned-tasks/unassigned-tasks.component';
import { TaskComponent } from './task/task.component';


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeComponent,
    CallbackComponent,
    RoleRequestComponent,
    GroupsComponent,
    GroupComponent,
    UserSearchComponent,
    UserComponent,
    UserGroupsComponent,
    WebMetricsComponent,
    EventTypeComponent,
    EventTypeListComponent,
    TaskTypeListComponent,
    TaskTypeGroupsComponent,
    TaskTypeEventTypesComponent,
    TaskTypeComponent,
    MyTasksComponent,
    UnassignedTasksComponent,
    TaskComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    AppRoutingModule
  ],
  providers: [AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
