import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccesspointTableComponent } from './accesspoint-table.component';

describe('AccesspointTableComponent', () => {
  let component: AccesspointTableComponent;
  let fixture: ComponentFixture<AccesspointTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccesspointTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccesspointTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
