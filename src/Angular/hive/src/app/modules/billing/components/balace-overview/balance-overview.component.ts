import { typeWithParameters } from '@angular/compiler/src/render3/util';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AccountHoldersClient, TransactionDto, WalletDto } from 'src/app/clients/billing-client';
import { UserProfileDto } from 'src/app/clients/profile-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { UpTopDialog } from '../up-top/up-top.component';

@Component({
  selector: 'app-balance-overview',
  templateUrl: './balance-overview.component.html',
  styleUrls: ['./balance-overview.component.scss']
})
export class BalanceOverviewComponent implements OnInit {
  @Input() wallet!: WalletDto;
  @Input() owner!: UserProfileDto;

  constructor(public dialog: MatDialog, private authService: AuthService) { }

  ngOnInit(): void {
  }

  openDepositDialog() {
    const dialogRef = this.dialog.open(UpTopDialog, {
      width: '30%',
      data: this.wallet
    })

    dialogRef.afterClosed()
      .subscribe((transactionDto: TransactionDto) => {
        if (transactionDto) {
          this.wallet.balance! += transactionDto.amount!;
        }
      })
  }

  get displayName() {
    const fullName = `${this.owner.firstName} ${this.owner.lastName}`;
    if (fullName.trim() === '') {
      return this.authService.user?.profile.email;
    }

    return fullName;
  }

}
