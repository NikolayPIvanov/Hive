import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionsOverviewComponent } from './transactions-overview.component';

describe('TransactionsOverviewComponent', () => {
  let component: TransactionsOverviewComponent;
  let fixture: ComponentFixture<TransactionsOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TransactionsOverviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TransactionsOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
