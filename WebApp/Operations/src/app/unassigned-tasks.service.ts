import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { UnassignedTask } from './unassigned-task';

@Injectable()
export class UnassignedTasksService {

  constructor(private http: Http) { }

  getByUser(): Promise<Array<UnassignedTask>> {
    return this.http.get(environment.baseUrl + "UnassignedTasks", {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.json());
  }
}
