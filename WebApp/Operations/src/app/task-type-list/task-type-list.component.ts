import { Component, OnInit } from '@angular/core';
import { TaskType } from '../task-type';
import { TaskTypesService } from '../task-types.service';
@Component({
  selector: 'app-task-type-list',
  templateUrl: './task-type-list.component.html',
  styleUrls: ['./task-type-list.component.css'],
  providers: [TaskTypesService]
})
export class TaskTypeListComponent implements OnInit {

  SpinnerHidden: boolean = true;
  TaskTypes: Array<TaskType> = null;

  constructor(private taskTypesService: TaskTypesService) { }

  ngOnInit() {
    this.SpinnerHidden = false;
    this.taskTypesService.getAllGroups()
    .then(taskTypes => {
      this.TaskTypes = taskTypes;
      this.SpinnerHidden = true;
    })
  }

}
