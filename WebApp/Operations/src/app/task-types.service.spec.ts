import { TestBed, inject } from '@angular/core/testing';

import { TaskTypesService } from './task-types.service';

describe('TaskTypesService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TaskTypesService]
    });
  });

  it('should be created', inject([TaskTypesService], (service: TaskTypesService) => {
    expect(service).toBeTruthy();
  }));
});
