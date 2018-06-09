import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Task } from './task';
@Injectable()
export class TaskService {

  constructor(private http: Http) { }

  getTask(taskId: string): Promise<Task> {
    return this.http.get(environment.baseUrl + "Task/" + taskId, {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.json() as Task)
  }

  getTaskFormIds(taskId: string): Promise<Array<string>> {
    return this.http.get(environment.baseUrl + "Task/" + taskId + "/FormIds", {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.json() as Array<string>)
  }

  closeTask(taskId: string): Promise<string> {
    return this.http.put(environment.baseUrl + "Task/" + taskId + "/Close", null, {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.text())
  }

}
