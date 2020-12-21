import { TestBed } from '@angular/core/testing';

import { ChartPreparationService } from './chart-preparation.service';

describe('ChartPreparationService', () => {
  let service: ChartPreparationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChartPreparationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
