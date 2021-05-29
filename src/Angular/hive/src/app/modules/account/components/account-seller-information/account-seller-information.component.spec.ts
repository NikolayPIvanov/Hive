import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSellerInformationComponent } from './account-seller-information.component';

describe('AccountSellerInformationComponent', () => {
  let component: AccountSellerInformationComponent;
  let fixture: ComponentFixture<AccountSellerInformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountSellerInformationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountSellerInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
