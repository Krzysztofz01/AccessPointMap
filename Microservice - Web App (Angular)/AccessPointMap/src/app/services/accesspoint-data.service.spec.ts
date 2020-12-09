import { TestBed } from '@angular/core/testing';

import { AccesspointDataService } from './accesspoint-data.service';

describe('AccesspointDataService', () => {
  let service: AccesspointDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccesspointDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
