import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlansSearchFieldComponent } from './plans-search-field.component';

describe('PlansSearchFieldComponent', () => {
  let component: PlansSearchFieldComponent;
  let fixture: ComponentFixture<PlansSearchFieldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlansSearchFieldComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlansSearchFieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
