import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigCardOverviewComponent } from './gig-card-overview.component';

describe('GigCardOverviewComponent', () => {
  let component: GigCardOverviewComponent;
  let fixture: ComponentFixture<GigCardOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigCardOverviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigCardOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
