import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigNameSearchComponent } from './gig-name-search.component';

describe('GigNameSearchComponent', () => {
  let component: GigNameSearchComponent;
  let fixture: ComponentFixture<GigNameSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigNameSearchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigNameSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
