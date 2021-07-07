import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { switchMap, tap } from 'rxjs/operators';
import { PlanDto } from 'src/app/clients/investing-client';
import { PlansService } from '../../services/plans.service';

@Component({
  selector: 'app-plan-edit',
  templateUrl: './plan-edit.component.html',
  styleUrls: ['./plan-edit.component.scss']
})
export class PlanEditComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<PlanEditComponent>,
    private planService: PlansService,
    private spinner: NgxSpinnerService,
    @Inject(MAT_DIALOG_DATA) public data: PlanDto
  ) { }

  form: FormGroup = this.fb.group({
    id: ['', Validators.required],
    isPublic: [false],
    title: ['', Validators.required],
    description: ['', Validators.required],
    fundingNeeded: [0, Validators.required],
    startDate: [null],
    endDate: [null]
  })

  ngOnInit(): void {
    this.form.patchValue(this.data)
  }

  formatLabel(value: number) {
    if (value >= 1000) {
      return Math.round(value / 1000) + 'k';
    }

    return value;
  }

  sliderChanged($event: any) {
    this.form.patchValue({ fundingNeeded: $event.value })
  }

  onSubmit() {
    this.spinner.show();
    this.planService.updatePlan(this.data.id!, this.form.value)
      .pipe(
        switchMap(() => this.planService.getPlan(this.data.id!)),
        tap({
          next: (plan) => {
            this.dialogRef.close(plan);
          },
        complete: () => {
          this.spinner.hide();
        }
      }))
      .subscribe();
  }

}
