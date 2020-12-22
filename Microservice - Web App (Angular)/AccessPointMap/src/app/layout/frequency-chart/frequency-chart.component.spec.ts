import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FrequencyChartComponent } from './frequency-chart.component';

describe('FrequencyChartComponent', () => {
  let component: FrequencyChartComponent;
  let fixture: ComponentFixture<FrequencyChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FrequencyChartComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FrequencyChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
