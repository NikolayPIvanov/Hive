import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { AccountHoldersClient, TransactionDto, WalletDto } from 'src/app/clients/billing-client';
import { UpTopDialog } from '../up-top/up-top.component';

@Component({
  selector: 'app-balance-overview',
  templateUrl: './balance-overview.component.html',
  styleUrls: ['./balance-overview.component.scss']
})
export class BalanceOverviewComponent implements OnInit {
  @Input() wallet!: WalletDto;

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  openDepositDialog() {
    this.dialog.open(UpTopDialog, {
      width: '30%',
      data: { accountHolder: this.wallet.accountHolderId, walletId: this.wallet.id }
    })
  }

}
