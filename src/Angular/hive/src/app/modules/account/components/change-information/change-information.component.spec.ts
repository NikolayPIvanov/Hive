import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeInformationComponent } from './change-information.component';

describe('ChangeInformationComponent', () => {
  let component: ChangeInformationComponent;
  let fixture: ComponentFixture<ChangeInformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangeInformationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
