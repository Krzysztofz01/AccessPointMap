import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAccesspointAddModalComponent } from './admin-accesspoint-add-modal.component';

describe('AdminAccesspointAddModalComponent', () => {
  let component: AdminAccesspointAddModalComponent;
  let fixture: ComponentFixture<AdminAccesspointAddModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminAccesspointAddModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAccesspointAddModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
