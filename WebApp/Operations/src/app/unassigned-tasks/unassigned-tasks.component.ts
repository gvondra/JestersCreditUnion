import { Component, OnInit } from '@angular/core';
import { UnassignedTasksService } from '../unassigned-tasks.service';
import { UnassignedTaskService } from '../unassigned-task.service';
import { UnassignedTask } from '../unassigned-task';

@Component({
  selector: 'app-unassigned-tasks',
  templateUrl: './unassigned-tasks.component.html',
  styleUrls: ['./unassigned-tasks.component.css'],
  providers: [UnassignedTasksService, UnassignedTaskService]
})
export class UnassignedTasksComponent implements OnInit {

  TaskTypes: Array<string> = null;  
  SelectedTaskType: string = null;
  Groups: Array<string> = null;
  Tasks: Array<UnassignedTask> = null;
  TasksFiltered: Array<UnassignedTask> = null;
  SpinnerHidden: boolean = false;
  Message: string = null;

  constructor(private unassignedTasksService: UnassignedTasksService, private unassignedTaskService: UnassignedTaskService) { }

  ngOnInit() {
    this.unassignedTasksService.getByUser()
    .then(tasks => {
      this.SetTasks(tasks);
      this.SpinnerHidden = true;
    })
  }

  ClaimTask(task: UnassignedTask): void {    
    this.SpinnerHidden = false;
    this.Message = null;
    this.unassignedTaskService.claimTask(task.TaskId)
    .then(message => {
      this.Message = message;
      let i: number = this.TasksFiltered.indexOf(task);
      this.TasksFiltered.splice(i, 1);
      i = this.Tasks.indexOf(task);
      this.Tasks.splice(i, 1);
      this.SpinnerHidden = true;
    })    
  }

  TaskTypeChange(event) {
    this.TasksFiltered = null;
    this.Message = null;
    this.Groups = ["-- Select --"];
    this.SelectedTaskType = event.target.value
    let i = 0;
    if (this.Tasks) {
      for (let t of this.Tasks) {
        if (t.TaskTypeTitle === this.SelectedTaskType && !this.Groups.includes(t.GroupName)) {
          this.Groups.push(t.GroupName);
          i += 1;
        }
      }
    }
    if (i === 0) {
      this.Groups = null;
    }
  }

  GroupChange(event) {
    this.TasksFiltered = [];
    this.Message = null;
    if (this.Tasks) {
      for (let t of this.Tasks) {
        if (t.TaskTypeTitle === this.SelectedTaskType && t.GroupName === event.target.value) {
          this.TasksFiltered.push(t);
        }
      }
    }    
  }

  FormatTimestamp(ts): string {
    if (ts && Date.parse(ts)) {
      let d = new Date(Date.parse(ts));
      ts = d.toLocaleDateString();
    }
    return ts
  }

  SetTasks(tasks: Array<UnassignedTask>): void {
    this.TasksFiltered = null; 
    this.Message = null;   
    let tt = ["-- Select --"];
    for (let t of tasks) {
      if (!tt.includes(t.TaskTypeTitle)) {
        tt.push(t.TaskTypeTitle);
      }
    }
    this.Tasks = tasks;
    this.TaskTypes = tt;
  }
}
