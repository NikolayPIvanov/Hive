import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoggedoutLayoutComponent } from './loggedout-layout.component';

describe('LoggedoutLayoutComponent', () => {
  let component: LoggedoutLayoutComponent;
  let fixture: ComponentFixture<LoggedoutLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoggedoutLayoutComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoggedoutLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
