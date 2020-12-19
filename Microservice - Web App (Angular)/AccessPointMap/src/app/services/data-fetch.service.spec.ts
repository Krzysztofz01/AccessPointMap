import { TestBed } from '@angular/core/testing';

import { DataFetchService } from './data-fetch.service';

describe('DataFetchService', () => {
  let service: DataFetchService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataFetchService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
