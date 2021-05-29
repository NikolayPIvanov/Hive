import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignedOutLayoutComponent } from './signed-out-layout.component';

describe('SignedOutLayoutComponent', () => {
  let component: SignedOutLayoutComponent;
  let fixture: ComponentFixture<SignedOutLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SignedOutLayoutComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SignedOutLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
