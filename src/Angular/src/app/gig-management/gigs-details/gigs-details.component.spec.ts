import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigsDetailsComponent } from './gigs-details.component';

describe('GigsDetailsComponent', () => {
  let component: GigsDetailsComponent;
  let fixture: ComponentFixture<GigsDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigsDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigsDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
