import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigsControlPanelComponent } from './gigs-control-panel.component';

describe('GigsControlPanelComponent', () => {
  let component: GigsControlPanelComponent;
  let fixture: ComponentFixture<GigsControlPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigsControlPanelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigsControlPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
