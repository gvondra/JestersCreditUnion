import 'rxjs/add/operator/switchMap';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { TaskType } from '../task-type';
import { TaskTypeEventType } from '../task-type-event-type';
import { TaskTypeService } from '../task-type.service';

@Component({
  selector: 'app-task-type-event-types',
  templateUrl: './task-type-event-types.component.html',
  styleUrls: ['./task-type-event-types.component.css'],
  providers: [TaskTypeService]
})
export class TaskTypeEventTypesComponent implements OnInit {

  TaskType: TaskType = null;  
  EventTypes: Array<TaskTypeEventType> = null;
  SpinnerHidden: boolean = true;
  Message: string = null;

  constructor(private route: ActivatedRoute, private taskTypeService: TaskTypeService) { }

  Submit(){
    this.SpinnerHidden = false;
    this.taskTypeService.putTaskTypeEventTypes(this.TaskType.TaskTypeId, this.EventTypes)
    .then(status => {
      this.Message = "Update Complete"
      this.SpinnerHidden = true;
    })
    .catch(ex => {
      console.log(ex);
      this.Message = "Update failed. " + ex.statusText;
      this.SpinnerHidden = true;
    })
  }

  ngOnInit() {
    this.route.params
    .switchMap((params: ParamMap) => {
      this.SpinnerHidden = false;
      this.EventTypes = null;
      return this.taskTypeService.getTaskType(params['id']);
    })
    .subscribe(taskType => {
      this.TaskType = taskType;
      if (taskType) {
        this.taskTypeService.getTaskTypeEventTypes(taskType.TaskTypeId)
        .then(eventTypes => {
            this.EventTypes = eventTypes;
            this.SpinnerHidden = true;
        })
      }
    });
  }

}
