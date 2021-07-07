import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigEditComponent } from './gig-edit.component';

describe('GigEditComponent', () => {
  let component: GigEditComponent;
  let fixture: ComponentFixture<GigEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
