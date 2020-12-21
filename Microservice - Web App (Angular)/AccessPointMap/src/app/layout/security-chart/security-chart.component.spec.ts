import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecurityChartComponent } from './security-chart.component';

describe('SecurityChartComponent', () => {
  let component: SecurityChartComponent;
  let fixture: ComponentFixture<SecurityChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SecurityChartComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SecurityChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
