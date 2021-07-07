import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigsControlComponent } from './gigs-control.component';

describe('GigsControlComponent', () => {
  let component: GigsControlComponent;
  let fixture: ComponentFixture<GigsControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigsControlComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigsControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
