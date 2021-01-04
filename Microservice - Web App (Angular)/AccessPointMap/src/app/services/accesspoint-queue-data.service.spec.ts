import { TestBed } from '@angular/core/testing';

import { AccesspointQueueDataService } from './accesspoint-queue-data.service';

describe('AccesspointQueueDataService', () => {
  let service: AccesspointQueueDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccesspointQueueDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
