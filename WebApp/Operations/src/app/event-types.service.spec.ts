import { TestBed, inject } from '@angular/core/testing';

import { EventTypesService } from './event-types.service';

describe('EventTypesService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EventTypesService]
    });
  });

  it('should be created', inject([EventTypesService], (service: EventTypesService) => {
    expect(service).toBeTruthy();
  }));
});
