import 'rxjs/add/operator/switchMap';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Task } from '../task';
import { TaskService } from '../task.service';
import { FormService } from '../form.service';
@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css'],
  providers: [ TaskService, FormService ]
})
export class TaskComponent implements OnInit {

  Task: Task = null;
  SpinnerHidden: boolean = true;
  FormsSpinnerHidden: boolean = true;
  FormContent: Array<string> = null;

  constructor(private route: ActivatedRoute, 
              private taskService: TaskService,
              private formService: FormService) { }

  FormatTimestamp(ts): string {
    if (ts && Date.parse(ts)) {
      let d = new Date(Date.parse(ts));
      ts = d.toLocaleString();
    }
    return ts
  }

  ngOnInit() {
    this.route.params
    .switchMap((params: ParamMap) => {
      this.SpinnerHidden = false;
      this.Task = null;
      return this.taskService.getTask(params['id']);
    })
    .subscribe(task => {
      this.Task = task;      
      this.LoadFormData();
      this.SpinnerHidden = true;
    });
  }

  LoadFormData() {
    this.FormContent = [];
    if (this.Task) {
      this.FormsSpinnerHidden = false;
      this.taskService.getTaskFormIds(this.Task.TaskId)
      .then(formIds => {
        let formCount: number = 0;
        if (formIds) {
          formCount = formIds.length;
        }
        console.log(formCount + " froms exist");
        this.GetNextFormHtml(formIds)
      })
    }    
  }

  GetNextFormHtml(formIds: Array<string>) {
    if (formIds && formIds.length > 0) {
      let i: string = formIds.pop()
      console.log("get html " + i)
      this.formService.getFormHtml(i)
      .then(h => {
        this.FormContent.push(h);
        console.log("got html " + i)
        this.GetNextFormHtml(formIds);        
      })
    }
    else {
      this.FormsSpinnerHidden = true;
    }
  }

  CloseTask() {
    this.taskService.closeTask(this.Task.TaskId)
    .then(msg => this.Task.IsClosed = true)
  }
}
