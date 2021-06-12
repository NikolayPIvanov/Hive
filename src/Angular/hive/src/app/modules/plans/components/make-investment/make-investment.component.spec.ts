import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MakeInvestmentComponent } from './make-investment.component';

describe('MakeInvestmentComponent', () => {
  let component: MakeInvestmentComponent;
  let fixture: ComponentFixture<MakeInvestmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MakeInvestmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MakeInvestmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
