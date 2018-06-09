import { TestBed, inject } from '@angular/core/testing';

import { UnassignedTaskService } from './unassigned-task.service';

describe('UnassignedTaskService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [UnassignedTaskService]
    });
  });

  it('should be created', inject([UnassignedTaskService], (service: UnassignedTaskService) => {
    expect(service).toBeTruthy();
  }));
});
