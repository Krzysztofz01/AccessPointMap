import { TestBed } from '@angular/core/testing';

import { PageAuthGuard } from './page-auth.guard';

describe('PageAuthGuard', () => {
  let guard: PageAuthGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(PageAuthGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
