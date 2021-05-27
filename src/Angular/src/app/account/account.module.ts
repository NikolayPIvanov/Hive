import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountOverviewComponent } from './account-overview/account-overview.component';
import { AccountRoutingModule } from './account-routing.module';
import { SharedModule } from '../shared/shared.module';
import { CoreModule } from '../core/core.module';
import { AvatarModule } from 'ngx-avatar';
import { AccountBalanceComponent } from './account-balance/account-balance.component';
import { AccountTransactionsComponent } from './account-transactions/account-transactions.component';
import { AccountBalanceFundComponent } from './account-balance-fund/account-balance-fund.component';
import { FundWalletDialogComponent } from './account-balance-fund/fund-wallet-dialog/fund-wallet-dialog.component';

@NgModule({
  declarations: [
    AccountOverviewComponent,
    AccountBalanceComponent,
    AccountTransactionsComponent,
    AccountBalanceFundComponent,
    FundWalletDialogComponent
  ],
  imports: [
    CommonModule,
    AccountRoutingModule,
    SharedModule,
    CoreModule,

    AvatarModule
  ]
})
export class AccountModule { }
