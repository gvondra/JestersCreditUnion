import { TestBed, inject } from '@angular/core/testing';

import { WebMetricService } from './web-metric.service';

describe('WebMetricService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WebMetricService]
    });
  });

  it('should be created', inject([WebMetricService], (service: WebMetricService) => {
    expect(service).toBeTruthy();
  }));
});
