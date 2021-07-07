import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SellerOverviewComponent } from './seller-overview.component';

describe('SellerOverviewComponent', () => {
  let component: SellerOverviewComponent;
  let fixture: ComponentFixture<SellerOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SellerOverviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SellerOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
