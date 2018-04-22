import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Group } from './group';

@Injectable()
export class GroupService {

  constructor(private http: Http) { }

  getAllGroups() : Promise<Array<Group>> {
    return this.http.get(environment.baseUrl + "Groups", {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.json() as Array<Group>)
  }

}
