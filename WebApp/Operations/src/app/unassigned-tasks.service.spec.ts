import { TestBed, inject } from '@angular/core/testing';

import { UnassignedTasksService } from './unassigned-tasks.service';

describe('UnassignedTasksService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UnassignedTasksService]
    });
  });

  it('should be created', inject([UnassignedTasksService], (service: UnassignedTasksService) => {
    expect(service).toBeTruthy();
  }));
});
