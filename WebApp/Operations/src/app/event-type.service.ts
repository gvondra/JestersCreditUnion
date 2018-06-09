import 'rxjs/add/operator/toPromise';
import { Headers, Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { EventType } from './event-type';

@Injectable()
export class EventTypeService {

  constructor(private http: Http) { }

  getEventType(eventTypeId: string) : Promise<EventType> {
    return this.http.get(environment.baseUrl + "EventType/" + eventTypeId, {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.json() as EventType)
  }

  putEventType(eventType: EventType): Promise<EventType> {
    return this.http.put(environment.baseUrl + "EventType", eventType,
    {
      headers: new Headers({"Authorization": `Bearer ${localStorage.getItem('token')}`})
    })
    .toPromise()
    .then(response => response.json() as EventType)
  }
}
