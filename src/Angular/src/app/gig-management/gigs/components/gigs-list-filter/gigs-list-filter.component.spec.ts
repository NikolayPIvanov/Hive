import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigsListFilterComponent } from './gigs-list-filter.component';

describe('GigsListFilterComponent', () => {
  let component: GigsListFilterComponent;
  let fixture: ComponentFixture<GigsListFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigsListFilterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigsListFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
