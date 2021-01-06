import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAccesspointMasterEditModalComponent } from './admin-accesspoint-master-edit-modal.component';

describe('AdminAccesspointMasterEditModalComponent', () => {
  let component: AdminAccesspointMasterEditModalComponent;
  let fixture: ComponentFixture<AdminAccesspointMasterEditModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminAccesspointMasterEditModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAccesspointMasterEditModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
