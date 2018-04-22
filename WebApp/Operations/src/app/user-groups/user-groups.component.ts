import 'rxjs/add/operator/switchMap';
import { Component, OnInit } from '@angular/core';
import { User } from '../user';
import { UserGroup } from '../user-group';
import { UserService } from '../user.service';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'app-user-groups',
  templateUrl: './user-groups.component.html',
  styleUrls: ['./user-groups.component.css'],
  providers: [UserService]
})
export class UserGroupsComponent implements OnInit {

  User: User = null;  
  Groups: Array<UserGroup> = null;
  SpinnerHidden: boolean = true;

  constructor(private route: ActivatedRoute, private userService: UserService) { }

  Submit() {
    this.SpinnerHidden = false;
    this.userService.putUserGroups(this.User.UserId, this.Groups)
    .then(status => this.SpinnerHidden = true)
  }

  ngOnInit() {
    this.route.params
    .switchMap((params: ParamMap) => {
      this.SpinnerHidden = false;
      this.Groups = null;
      return this.userService.getUser(params['id']);
    })
    .subscribe(user => {
      this.User = user;
      if (user) {
        this.userService.getUserGroups(user.UserId)
        .then(groups => {
            this.Groups = groups;
            this.SpinnerHidden = true;
        })
      }
    });
  }

}
