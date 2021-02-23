import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAccesspointQueueModalComponent } from './admin-accesspoint-queue-modal.component';

describe('AdminAccesspointQueueModalComponent', () => {
  let component: AdminAccesspointQueueModalComponent;
  let fixture: ComponentFixture<AdminAccesspointQueueModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminAccesspointQueueModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAccesspointQueueModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
