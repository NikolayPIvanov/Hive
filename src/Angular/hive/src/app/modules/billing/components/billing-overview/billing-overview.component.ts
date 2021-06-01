import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AccountHoldersClient, TransactionDto, TransactionType, WalletDto } from 'src/app/clients/billing-client';

@Component({
  selector: 'app-billing-overview',
  templateUrl: './billing-overview.component.html',
  styleUrls: ['./billing-overview.component.scss']
})
export class BillingOverviewComponent implements OnInit {
  wallet$!: Observable<WalletDto>;

  constructor(private billingApiClient: AccountHoldersClient) { }
  ngOnInit(): void {
    this.wallet$ = of(
      new WalletDto({
        id: 1,
        accountHolderId: 1,
        balance: 100.0,
        transactions: [
          new TransactionDto({ amount: 100.0, transactionNumber: 1, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 2, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 3, transactionType: TransactionType.Payment, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 100.0, transactionNumber: 1, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 2, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 3, transactionType: TransactionType.Payment, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 100.0, transactionNumber: 1, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 2, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 3, transactionType: TransactionType.Payment, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 100.0, transactionNumber: 1, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 2, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 3, transactionType: TransactionType.Payment, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 100.0, transactionNumber: 1, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 2, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 3, transactionType: TransactionType.Payment, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 100.0, transactionNumber: 1, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 2, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 3, transactionType: TransactionType.Payment, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 100.0, transactionNumber: 1, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 2, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 3, transactionType: TransactionType.Payment, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 100.0, transactionNumber: 1, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 2, transactionType: TransactionType.Fund, walletId: 1, orderNumber: undefined }),
          new TransactionDto({ amount: 20.0, transactionNumber: 3, transactionType: TransactionType.Payment, walletId: 1, orderNumber: undefined }),
        ]
      })
    )
      // this.billingApiClient.getWallet();
  }

}
