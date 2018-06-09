import 'rxjs/add/operator/switchMap';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { EventType } from '../event-type';
import { EventTypeService } from '../event-type.service';

@Component({
  selector: 'app-event-type',
  templateUrl: './event-type.component.html',
  styleUrls: ['./event-type.component.css'],
  providers: [EventTypeService]
})
export class EventTypeComponent implements OnInit {

  SpinnerHidden: boolean = true;
  EventType: EventType = null;

  constructor(private route: ActivatedRoute, private eventTypeService: EventTypeService) { }

  Submit() {
    this.SpinnerHidden = false;
    this.eventTypeService.putEventType(this.EventType)
    .then(eventType => {
      this.EventType = eventType;
      this.SpinnerHidden = true;
    })
  }

  ngOnInit() {
    this.route.params
    .switchMap((params: ParamMap) => {
      this.SpinnerHidden = false;
      return this.eventTypeService.getEventType(params['id']);
    })
    .subscribe(eventType => {
      this.EventType = eventType;
      this.SpinnerHidden = true;
    });
  }

}
