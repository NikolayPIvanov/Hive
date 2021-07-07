import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigDashboardComponent } from './gig-dashboard.component';

describe('GigDashboardComponent', () => {
  let component: GigDashboardComponent;
  let fixture: ComponentFixture<GigDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigDashboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
