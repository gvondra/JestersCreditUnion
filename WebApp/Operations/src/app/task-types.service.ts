import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { TaskType } from './task-type';

@Injectable()
export class TaskTypesService {

  constructor(private http: Http) { }

  getAllGroups() : Promise<Array<TaskType>> {
    return this.http.get(environment.baseUrl + "TaskTypes", {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.json() as Array<TaskType>)
  }
}
