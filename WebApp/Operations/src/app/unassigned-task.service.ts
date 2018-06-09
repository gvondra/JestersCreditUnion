import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';

@Injectable()
export class UnassignedTaskService {

  constructor(private http: Http) { }

  claimTask(taskId: string) : Promise<string> {
    return this.http.put(environment.baseUrl + "UnassignedTask/" + taskId + "/Claim", null,
    {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.text() as string)
  }

}
