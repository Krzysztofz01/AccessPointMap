import { TestBed } from '@angular/core/testing';

import { DateFormatingService } from './date-formating.service';

describe('DateFormatingService', () => {
  let service: DateFormatingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DateFormatingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
