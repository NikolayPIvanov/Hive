import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigsSingleCardComponent } from './gigs-single-card.component';

describe('GigsSingleCardComponent', () => {
  let component: GigsSingleCardComponent;
  let fixture: ComponentFixture<GigsSingleCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigsSingleCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigsSingleCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
