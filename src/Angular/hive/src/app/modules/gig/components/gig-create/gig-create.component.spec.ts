import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GigCreateComponent } from './gig-create.component';

describe('GigCreateComponent', () => {
  let component: GigCreateComponent;
  let fixture: ComponentFixture<GigCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GigCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GigCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
