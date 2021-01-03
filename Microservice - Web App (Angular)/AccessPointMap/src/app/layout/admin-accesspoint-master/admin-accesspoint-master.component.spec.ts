import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAccesspointMasterComponent } from './admin-accesspoint-master.component';

describe('AdminAccesspointMasterComponent', () => {
  let component: AdminAccesspointMasterComponent;
  let fixture: ComponentFixture<AdminAccesspointMasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminAccesspointMasterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAccesspointMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
