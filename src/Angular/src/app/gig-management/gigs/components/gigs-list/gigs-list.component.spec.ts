import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigsListComponent } from './gigs-list.component';

describe('GigsListComponent', () => {
  let component: GigsListComponent;
  let fixture: ComponentFixture<GigsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
