import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CreditCardValidators } from 'angular-cc-library';
import { tap } from 'rxjs/operators';
import { AccountHoldersClient, CreateTransactionCommand, WalletDto } from 'src/app/clients/billing-client';

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
    this.submitted = true;
    const command = CreateTransactionCommand.fromJS(this.form.value)
    this.billingClient.depositInWallet(command.walletId!,
      this.data.accountHolderId?.toString()!, command)
      .pipe(
        tap({
          next: (response) => this.dialogRef.close(response),
          complete: () => this.dialogRef.close(command.amount)
        }))
      .subscribe();
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
