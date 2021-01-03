import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminAccesspointQueueComponent } from './admin-accesspoint-queue.component';

describe('AdminAccesspointQueueComponent', () => {
  let component: AdminAccesspointQueueComponent;
  let fixture: ComponentFixture<AdminAccesspointQueueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminAccesspointQueueComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminAccesspointQueueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
