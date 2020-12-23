import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageMainComponent } from './page-main.component';

describe('PageMainComponent', () => {
  let component: PageMainComponent;
  let fixture: ComponentFixture<PageMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageMainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
