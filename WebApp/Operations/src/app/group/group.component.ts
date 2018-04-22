import 'rxjs/add/operator/switchMap';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Group } from '../group';
import { GroupService } from '../group.service';
@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css'],
  providers: [GroupService]
})
export class GroupComponent implements OnInit {

  Group: Group = null;
  SpinnerHidden: boolean = true;

  constructor(private route: ActivatedRoute, private groupService: GroupService) { }

  Submit() {
    this.SpinnerHidden = false;
    this.groupService.putGroup(this.Group)
    .then(group => {
      this.Group = group;
      this.SpinnerHidden = true;
    })
  }

  ngOnInit() {
    this.route.params
    .switchMap((params: ParamMap) => { 
      this.SpinnerHidden = false;
      if (params['id']) {        
        return this.groupService.getGroup(params['id']);
      }
      else {
        return Promise.resolve(new Group());
      }      
    })
    .subscribe(group => {
      this.Group = group;    
      this.SpinnerHidden = true;
    });
  }

}
