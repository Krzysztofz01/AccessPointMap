import { TestBed } from '@angular/core/testing';

import { AccessPointDeatilModalService } from './access-point-deatil-modal.service';

describe('AccessPointDeatilModalService', () => {
  let service: AccessPointDeatilModalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccessPointDeatilModalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
