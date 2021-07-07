import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutSidenavComponent } from './layout-sidenav.component';

describe('LayoutSidenavComponent', () => {
  let component: LayoutSidenavComponent;
  let fixture: ComponentFixture<LayoutSidenavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LayoutSidenavComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutSidenavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
