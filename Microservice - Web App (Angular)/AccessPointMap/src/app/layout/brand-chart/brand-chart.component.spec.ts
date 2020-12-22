import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BrandChartComponent } from './brand-chart.component';

describe('BrandChartComponent', () => {
  let component: BrandChartComponent;
  let fixture: ComponentFixture<BrandChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BrandChartComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BrandChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
