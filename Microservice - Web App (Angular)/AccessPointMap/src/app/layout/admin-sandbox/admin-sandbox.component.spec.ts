import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminSandboxComponent } from './admin-sandbox.component';

describe('AdminSandboxComponent', () => {
  let component: AdminSandboxComponent;
  let fixture: ComponentFixture<AdminSandboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminSandboxComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminSandboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
