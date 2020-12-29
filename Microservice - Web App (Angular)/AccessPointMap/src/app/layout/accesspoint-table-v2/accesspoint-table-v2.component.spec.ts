import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccesspointTableV2Component } from './accesspoint-table-v2.component';

describe('AccesspointTableV2Component', () => {
  let component: AccesspointTableV2Component;
  let fixture: ComponentFixture<AccesspointTableV2Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccesspointTableV2Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccesspointTableV2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
