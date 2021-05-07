import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigsCreateComponent } from './gigs-create.component';

describe('GigsCreateComponent', () => {
  let component: GigsCreateComponent;
  let fixture: ComponentFixture<GigsCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigsCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
