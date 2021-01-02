import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageUserComponent } from './page-user.component';

describe('PageUserComponent', () => {
  let component: PageUserComponent;
  let fixture: ComponentFixture<PageUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageUserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
