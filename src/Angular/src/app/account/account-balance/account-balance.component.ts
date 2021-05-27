import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountHoldersClient, WalletDto } from 'src/app/clients/billing-client';

@Component({
  selector: 'app-account-balance',
  templateUrl: './account-balance.component.html',
  styleUrls: ['./account-balance.component.scss']
})
export class AccountBalanceComponent implements OnInit {
  private walletBalance: number = 0.0;
  wallet$!: Observable<WalletDto>;

  constructor(private billingClient: AccountHoldersClient) { }

  ngOnInit(): void {
    this.wallet$ = this.billingClient.getWallet()
      .pipe(map(wallet => {
        this.walletBalance = wallet.balance!;
        return wallet;
      }));
  }

  get balance(): string {
    if (this.walletBalance < 1000.0) {
      return this.walletBalance.toString();
    }

    return (Math.round((this.walletBalance / 1000.0))+ "k");
  }

}
