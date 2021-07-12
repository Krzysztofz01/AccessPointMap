import { TestBed } from '@angular/core/testing';

import { PermissionErrorInterceptor } from './permission-error.interceptor';

describe('PermissionErrorInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      PermissionErrorInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: PermissionErrorInterceptor = TestBed.inject(PermissionErrorInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
