import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BillingOverviewComponent } from './components/billing-overview/billing-overview.component';
import { LayoutModule } from '../layout/layout.module';
import { BillingRoutingModule } from './billing-routing.module';
import { TransactionsOverviewComponent } from './components/transactions-overview/transactions-overview.component';
import { BalanceOverviewComponent } from './components/balace-overview/balance-overview.component';
import { UpTopDialog } from './components/up-top/up-top.component';



@NgModule({
  declarations: [
    BillingOverviewComponent,
    BalanceOverviewComponent,
    TransactionsOverviewComponent,
    UpTopDialog
  ],
  imports: [
    CommonModule,

    LayoutModule,

    BillingRoutingModule
  ]
})
export class BillingModule { }
