import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TransactionDto, WalletDto } from 'src/app/clients/billing-client';
import { NotificationService } from 'src/app/core/services/notification.service';
import { FundWalletDialogComponent } from './fund-wallet-dialog/fund-wallet-dialog.component';

@Component({
  selector: 'app-account-balance-fund',
  templateUrl: './account-balance-fund.component.html',
  styleUrls: ['./account-balance-fund.component.scss']
})
export class AccountBalanceFundComponent {
  @Input('wallet') wallet!: WalletDto;
  @Output('createdTransaction') emitter = new EventEmitter<TransactionDto>();

  constructor(
    private notificationService: NotificationService,
    public dialog: MatDialog) { }

  openDialog(): void {
    const dialogRef = this.dialog.open(FundWalletDialogComponent, {
      width: '250px',
      data: this.wallet
    });

    dialogRef.afterClosed().subscribe(result => {
      this.notificationService.openSnackBar('Funding placed!')
      this.emitter.emit(result);
    });
  }
}
