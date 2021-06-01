import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigCardComponent } from './gig-card.component';

describe('GigCardComponent', () => {
  let component: GigCardComponent;
  let fixture: ComponentFixture<GigCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
