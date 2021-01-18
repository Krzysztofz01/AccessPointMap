import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAccesspointAddComponent } from './admin-accesspoint-add.component';

describe('AdminAccesspointAddComponent', () => {
  let component: AdminAccesspointAddComponent;
  let fixture: ComponentFixture<AdminAccesspointAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminAccesspointAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAccesspointAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
