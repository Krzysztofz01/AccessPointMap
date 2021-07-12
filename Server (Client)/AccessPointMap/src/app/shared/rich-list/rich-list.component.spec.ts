import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RichListComponent } from './rich-list.component';

describe('RichListComponent', () => {
  let component: RichListComponent;
  let fixture: ComponentFixture<RichListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RichListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RichListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
