import { TestBed } from '@angular/core/testing';

import { SelectedAccesspointService } from './selected-accesspoint.service';

describe('SelectedAccesspointService', () => {
  let service: SelectedAccesspointService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SelectedAccesspointService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
