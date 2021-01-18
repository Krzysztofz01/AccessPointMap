import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccesspointViewModalComponent } from './accesspoint-view-modal.component';

describe('AccesspointViewModalComponent', () => {
  let component: AccesspointViewModalComponent;
  let fixture: ComponentFixture<AccesspointViewModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccesspointViewModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccesspointViewModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
