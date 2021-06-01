import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CreditCardValidators } from 'angular-cc-library';

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
    public dialogRef: MatDialogRef<UpTopDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
    this.form = this._fb.group({
      amount: ['', [Validators.required, Validators.min(1.0)]],
      creditCard: ['', [CreditCardValidators.validateCCNumber]],
      expirationDate: ['', [CreditCardValidators.validateExpDate]],
      cvc: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(4)]] 
    });
  }

  onSubmit(form: any) {
    this.submitted = true;
    console.log(form);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
