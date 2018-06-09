import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { EventType } from './event-type';

@Injectable()
export class EventTypesService {

  constructor(private http: Http) { }

  getAllEventTyes() : Promise<Array<EventType>> {
    return this.http.get(environment.baseUrl + "EventTypes", {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.json() as Array<EventType>)
  }

}
