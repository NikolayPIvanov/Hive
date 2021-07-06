import { Inject } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MakeInvestmentCommand, PlanDto, PlansClient } from 'src/app/clients/investing-client';

@Component({
  selector: 'app-make-investment',
  templateUrl: './make-investment.component.html',
  styleUrls: ['./make-investment.component.scss']
})
export class MakeInvestmentComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private planClient: PlansClient,
    public dialogRef: MatDialogRef<MakeInvestmentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PlanDto) { }

  form = this.fb.group({
    effectiveDate: [this.data.startDate!],
    amount: [0, Validators.required],
    roiPercentage: [0, Validators.required],
    planId: [this.data.id]
  });

  ngOnInit(): void {
  }

  formatLabel(value: number) {
    if (value >= 1000) {
      return Math.round(value / 1000) + 'k';
    }

    return value;
  }

  onSubmit() {
    const command = MakeInvestmentCommand.fromJS(this.form.value);
    this.planClient.makeInvestment(this.data.id!, command)
      .subscribe(investment => this.dialogRef.close(investment));
  }

}
