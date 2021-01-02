import { TestBed } from '@angular/core/testing';

import { PageAdminGuard } from './page-admin.guard';

describe('PageAdminGuard', () => {
  let guard: PageAdminGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(PageAdminGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
