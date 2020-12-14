import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GlobalMapComponent } from './global-map.component';

describe('GlobalMapComponent', () => {
  let component: GlobalMapComponent;
  let fixture: ComponentFixture<GlobalMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GlobalMapComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GlobalMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
