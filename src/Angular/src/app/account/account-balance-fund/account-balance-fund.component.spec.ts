import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountBalanceFundComponent } from './account-balance-fund.component';

describe('AccountBalanceFundComponent', () => {
  let component: AccountBalanceFundComponent;
  let fixture: ComponentFixture<AccountBalanceFundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountBalanceFundComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountBalanceFundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
