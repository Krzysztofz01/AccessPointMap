import { TestBed } from '@angular/core/testing';

import { PageUserGuard } from './page-user.guard';

describe('PageUserGuard', () => {
  let guard: PageUserGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(PageUserGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
