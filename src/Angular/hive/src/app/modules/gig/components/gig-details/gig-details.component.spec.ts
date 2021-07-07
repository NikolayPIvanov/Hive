import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigDetailsComponent } from './gig-details.component';

describe('GigDetailsComponent', () => {
  let component: GigDetailsComponent;
  let fixture: ComponentFixture<GigDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
