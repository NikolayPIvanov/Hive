import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpTopComponent } from './up-top.component';

describe('UpTopComponent', () => {
  let component: UpTopComponent;
  let fixture: ComponentFixture<UpTopComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpTopComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UpTopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
