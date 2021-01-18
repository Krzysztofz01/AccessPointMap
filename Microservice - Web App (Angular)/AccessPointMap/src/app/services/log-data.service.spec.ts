import { TestBed } from '@angular/core/testing';

import { LogDataService } from './log-data.service';

describe('LogDataService', () => {
  let service: LogDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LogDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
