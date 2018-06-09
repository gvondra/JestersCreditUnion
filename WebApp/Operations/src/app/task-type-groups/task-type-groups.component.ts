import 'rxjs/add/operator/switchMap';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { TaskTypeService } from '../task-type.service';
import { TaskType } from '../task-type';
import { TaskTypeGroup } from '../task-type-group';

@Component({
  selector: 'app-task-type-groups',
  templateUrl: './task-type-groups.component.html',
  styleUrls: ['./task-type-groups.component.css'],
  providers: [TaskTypeService]
})
export class TaskTypeGroupsComponent implements OnInit {

  TaskType: TaskType = null;  
  Groups: Array<TaskTypeGroup> = null;
  SpinnerHidden: boolean = true;

  constructor(private route: ActivatedRoute, private taskTypeService: TaskTypeService) { }

  Submit() {
    this.SpinnerHidden = false;
    this.taskTypeService.putTaskTypeGroups(this.TaskType.TaskTypeId, this.Groups)
    .then(status => this.SpinnerHidden = true)
  }

  ngOnInit() {
    this.route.params
    .switchMap((params: ParamMap) => {
      this.SpinnerHidden = false;
      this.Groups = null;
      return this.taskTypeService.getTaskType(params['id']);
    })
    .subscribe(taskType => {
      this.TaskType = taskType;
      if (taskType) {
        this.taskTypeService.getTaskTypeGroups(taskType.TaskTypeId)
        .then(groups => {
            this.Groups = groups;
            this.SpinnerHidden = true;
        })
      }
    });
  }

}
