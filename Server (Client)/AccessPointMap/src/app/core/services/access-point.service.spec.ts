import { TestBed } from '@angular/core/testing';

import { AccessPointService } from './access-point.service';

describe('AccessPointService', () => {
  let service: AccessPointService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccessPointService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
