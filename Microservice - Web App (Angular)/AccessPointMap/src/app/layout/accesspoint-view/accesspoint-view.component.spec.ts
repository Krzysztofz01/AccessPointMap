import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccesspointViewComponent } from './accesspoint-view.component';

describe('AccesspointViewComponent', () => {
  let component: AccesspointViewComponent;
  let fixture: ComponentFixture<AccesspointViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccesspointViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccesspointViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
