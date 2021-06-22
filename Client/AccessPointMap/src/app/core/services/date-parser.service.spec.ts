import { TestBed } from '@angular/core/testing';

import { DateParserService } from './date-parser.service';

describe('DateParserService', () => {
  let service: DateParserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DateParserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
