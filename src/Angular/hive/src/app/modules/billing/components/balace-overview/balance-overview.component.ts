import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AccountHoldersClient, TransactionDto, WalletDto } from 'src/app/clients/billing-client';
import { UserProfileDto } from 'src/app/clients/profile-client';
import { UpTopDialog } from '../up-top/up-top.component';

@Component({
  selector: 'app-balance-overview',
  templateUrl: './balance-overview.component.html',
  styleUrls: ['./balance-overview.component.scss']
})
export class BalanceOverviewComponent implements OnInit {
  @Input() wallet!: WalletDto;
  @Input() owner!: UserProfileDto;

  @Output() onTransactionAdded = new EventEmitter<number>();

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    debugger;
    console.log(this.owner)
  }

  openDepositDialog() {
    const dialogRef = this.dialog.open(UpTopDialog, {
      width: '30%',
      data: this.wallet
    })

    dialogRef.afterClosed()
      .subscribe(depositedAmount => {
        if (depositedAmount) {
          this.wallet.balance += depositedAmount;
          this.onTransactionAdded.emit(0);
        }
      })
  }

}
