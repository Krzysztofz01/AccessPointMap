import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageCheckComponent } from './page-check.component';

describe('PageCheckComponent', () => {
  let component: PageCheckComponent;
  let fixture: ComponentFixture<PageCheckComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageCheckComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
