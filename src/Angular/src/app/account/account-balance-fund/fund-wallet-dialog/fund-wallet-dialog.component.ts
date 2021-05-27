import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AccountHoldersClient, CreateTransactionCommand, WalletDto } from 'src/app/clients/billing-client';



@Component({
  selector: 'app-fund-wallet-dialog',
  templateUrl: './fund-wallet-dialog.component.html',
  styleUrls: ['./fund-wallet-dialog.component.scss']
})
export class FundWalletDialogComponent implements OnInit, OnDestroy {
  private subject = new Subject();
  fundControl = new FormControl(1.0, [
    Validators.required,
    Validators.min(1),
    Validators.max(10000)
  ]);
  
  constructor(
    private billingApiClient: AccountHoldersClient,
    public dialogRef: MatDialogRef<FundWalletDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: WalletDto) { }
  
  ngOnInit(): void {
  }
  
  ngOnDestroy(): void {
    this.subject.next();
    this.subject.complete();
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  fund(): void {
    debugger;
    const data = { walletId: this.data.id, amount: +this.fundControl.value };
    const command = CreateTransactionCommand.fromJS(data)
    this.billingApiClient
      .depositInWallet(this.data.id!, this.data.accountHolderId?.toString()!, command)
      .pipe(takeUntil(this.subject))
      .subscribe(publicId => { });
  }
}
