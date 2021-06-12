import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CreditCardValidators } from 'angular-cc-library';
import { switchMap, tap } from 'rxjs/operators';
import { AccountHoldersClient, CreateTransactionCommand, WalletDto } from 'src/app/clients/billing-client';
import { BillingService } from '../../services/billing.service';

@Component({
  selector: 'app-up-top',
  templateUrl: './up-top.component.html',
  styleUrls: ['./up-top.component.scss']
})
export class UpTopDialog implements OnInit {
  form!: FormGroup;
  submitted: boolean = false;

constructor(
  private _fb: FormBuilder,
  private billingService: BillingService,
  private billingClient: AccountHoldersClient,
  public dialogRef: MatDialogRef<UpTopDialog>,
  @Inject(MAT_DIALOG_DATA) public data: WalletDto
  ) { }

  ngOnInit(): void {
    this.form = this._fb.group({
      amount: ['', [Validators.required, Validators.min(1.0)]],
      walletId: [this.data.id, Validators.required]
    });
  }

  onSubmit() {
    const command = CreateTransactionCommand.fromJS(this.form.value)
    this.billingClient.depositInWallet(this.data.accountHolderId!, command.walletId!, command)
      .pipe(
        switchMap((trans: any) => {
          return this.billingClient.getTransactionById(this.data.accountHolderId!, this.data.id!, trans.transactionId)
        }),
        tap({
          next: (transaction) => {
            this.billingService.onNewTransaction(transaction);
            this.dialogRef.close(transaction);
          }
        }))
      .subscribe();
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
