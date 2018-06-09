import { Component, OnInit } from '@angular/core';
import { Task } from '../task';
import { TasksService } from '../tasks.service';

@Component({
  selector: 'app-my-tasks',
  templateUrl: './my-tasks.component.html',
  styleUrls: ['./my-tasks.component.css'],
  providers: [TasksService]
})
export class MyTasksComponent implements OnInit {

  Tasks: Array<Task> = null;
  SpinnerHidden: boolean = false;

  constructor(private tasksService: TasksService) { }

  ngOnInit() {
    this.tasksService.getMyTasks()
    .then(tasks => {
      this.Tasks = tasks;
      this.SpinnerHidden = true;
    })
  }

  FormatTimestamp(ts): string {
    if (ts && Date.parse(ts)) {
      let d = new Date(Date.parse(ts));
      ts = d.toLocaleDateString();
    }
    return ts
  }

}
