import { Component, OnInit } from '@angular/core';
import { EventType } from '../event-type';
import { EventTypesService } from '../event-types.service';
@Component({
  selector: 'app-event-type-list',
  templateUrl: './event-type-list.component.html',
  styleUrls: ['./event-type-list.component.css'],
  providers: [EventTypesService]
})
export class EventTypeListComponent implements OnInit {

  SpinnerHidden: boolean = true;
  EventTypes: Array<EventType> = null;

  constructor(private eventTypesService: EventTypesService) { }

  ngOnInit() {
    this.SpinnerHidden = false;
    this.eventTypesService.getAllEventTyes()
    .then(eventTypes => {
      this.EventTypes = eventTypes;
      this.SpinnerHidden = true;
    })
  }

}
