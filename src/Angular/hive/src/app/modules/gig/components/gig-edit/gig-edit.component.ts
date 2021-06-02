import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { tap } from 'rxjs/operators';
import { GigDto, GigsClient, UpdateGigCommand } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-gig-edit',
  templateUrl: './gig-edit.component.html',
  styleUrls: ['./gig-edit.component.scss']
})
export class GigEditComponent implements OnInit {
  @Input() gig!: GigDto;

  form = this.fb.group({
    id: ['', Validators.required],
    title: ['', Validators.required],
    description: ['', Validators.required],
    categoryId: ['', Validators.required],
    isDraft: ['', Validators.required],
    questions: this.fb.array([ this.initQuestion() ]),
  });

  constructor(
    public dialogRef: MatDialogRef<GigEditComponent>,
    private fb: FormBuilder,
    private gigsApiClient: GigsClient,
    private spinnerService: NgxSpinnerService) { }

  ngOnInit(): void {
    this.form.patchValue(this.gig);
  }

  onSubmit() {
    this.spinnerService.show();
    console.log(this.form.value);
    const command = UpdateGigCommand.fromJS(this.form.value)
    this.gigsApiClient.update(this.form.get('id')!.value, command)
      .pipe(tap({
        complete: () => {
          this.spinnerService.hide();
          this.dialogRef.close();
        }
      }))
  }

  getErrorMessage(key: string, error: string = 'required') {
    const control = this.form.get(key)!;
    if (control.hasError('required')) {
      return 'You must enter a value';
    }

    return control.hasError('email') ? 'Not a valid email' : '';
  }

  addQuestion() {
    const control = <FormArray>this.form.controls['questions'];
    control.push(this.initQuestion());
  }

  removeQuestion(i: number) {
    const control = <FormArray>this.form.controls['questions'];
    control.removeAt(i);
  }

  private initQuestion() {
    return this.fb.group({
        title: ['', Validators.required],
        answer: ['']
    });
  }

}
