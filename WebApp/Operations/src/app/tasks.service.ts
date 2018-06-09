import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Task } from './task';

@Injectable()
export class TasksService {

  constructor(private http: Http) { }

  getMyTasks() : Promise<Array<Task>> {
    let query = new URLSearchParams();
    query.append("open", "true");
    return this.http.get(environment.baseUrl + "Tasks", {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`}),
      params: query
    })
    .toPromise()
    .then(response => response.json() as Array<Task>)
  }
}
