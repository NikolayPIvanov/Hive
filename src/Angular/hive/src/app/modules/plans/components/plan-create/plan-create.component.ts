import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';
import { tap } from 'rxjs/operators';
import { CreatePlanCommand } from 'src/app/clients/investing-client';
import { PlansService } from '../../services/plans.service';

@Component({
  selector: 'app-plan-create',
  templateUrl: './plan-create.component.html',
  styleUrls: ['./plan-create.component.scss']
})
export class PlanCreateComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<PlanCreateComponent>,
    private planService: PlansService,
    private spinner: NgxSpinnerService) { }

  form: FormGroup = this.fb.group({
    title: ['', Validators.required],
    description: ['', Validators.required],
    fundingNeeded: [0, Validators.required],
    startDate: [null],
    endDate: [null]
  })

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
    this.planService.createPlan(this.form.value)
      .pipe(tap({
        next: (plan) => {
          this.dialogRef.close(plan);
        },
        complete: () => {
          this.spinner.hide();
        }
      }))
      .subscribe();
  }


  ngOnInit(): void {
  }

}
