import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MergeModalComponent } from './merge-modal.component';

describe('MergeModalComponent', () => {
  let component: MergeModalComponent;
  let fixture: ComponentFixture<MergeModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MergeModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MergeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
