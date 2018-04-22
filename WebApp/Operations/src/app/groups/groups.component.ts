import { Component, OnInit } from '@angular/core';
import { Group } from '../group';
import { GroupService } from '../group.service';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css'],
  providers: [GroupService]
})
export class GroupsComponent implements OnInit {

  Groups: Array<Group> = null;
  SpinnerHidden: boolean = true;

  constructor(private groupService: GroupService) { }

  ngOnInit() {
    this.SpinnerHidden = false;
    this.groupService.getAllGroups()
    .then(groups => {
      this.Groups = groups;
      this.SpinnerHidden = true;
    })
  }

}
