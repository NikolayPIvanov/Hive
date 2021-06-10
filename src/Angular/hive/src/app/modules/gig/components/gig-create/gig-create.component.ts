import { ENTER, COMMA } from '@angular/cdk/keycodes';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { MatDialogRef } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, of, Subject, throwError } from 'rxjs';
import { map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CategoriesClient, CreateGigCommand, FileUpload, GigsClient } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-gig-create',
  templateUrl: './gig-create.component.html',
  styleUrls: ['./gig-create.component.scss']
})
export class GigCreateComponent implements OnInit {
  // Shared
  private unsubscribe = new Subject();
  isLinear = true;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  constructor(
    public dialogRef: MatDialogRef<GigCreateComponent>,
    private fb: FormBuilder,
    private spinnerService: NgxSpinnerService,
    private gigsApiClient: GigsClient,
    private categoryApiClient: CategoriesClient) { }

  ngOnInit(): void { }

  onNoClick(): void {
    this.dialogRef.close();
  }
  setImage(base64: string) {
    this.gigForm.patchValue({ image: base64 });
  }

  // Gig Main Information
  gigForm = this.fb.group({
    title: ['', Validators.required],
    description: ['', Validators.required],
    categoryId: ['', Validators.required],
    planId: [null],
    tags: [[]],
    questions: this.fb.array([this.initQuestion()]),
    image: [null],
    packages: this.fb.array([ this.initPackage() ]),
  });

  getErrorMessage(key: string, error: string = 'required') {
    const control = this.gigForm.get(key)!;
    if (control.hasError('required')) {
      return 'You must enter a value';
    }

    return control.hasError('email') ? 'Not a valid email' : '';
  }

  onSubmit() {
    const command = CreateGigCommand.fromJS(this.gigForm.value);
      this.gigsApiClient.createGig(command)
        .pipe(
          takeUntil(this.unsubscribe),
          switchMap((id) => {
            const imageData = this.gigForm.get('image');
            if (imageData) {
              return this.gigsApiClient.updateImage(id, FileUpload.fromJS({ fileData: imageData.value }))
            }

            return of(id);
          }),
          tap({
            complete: () => this.onNoClick()
          }))
        .subscribe();
  }

  addQuestion() {
    const control = <FormArray>this.gigForm.controls['questions'];
    control.push(this.initQuestion());
  }

  removeQuestion(i: number) {
    const control = <FormArray>this.gigForm.controls['questions'];
    control.removeAt(i);
  }

  private initQuestion() {
    return this.fb.group({
        title: ['', Validators.required],
        answer: ['', Validators.required]
    });
  }

  get tags(): FormControl { 
    return this.gigForm.get('tags') as FormControl;
  }

  add(control: FormControl, event: MatChipInputEvent) {
    const value = (event.value || '').trim();

    if (value) {
      control.value.push(value);
      control.updateValueAndValidity();
    }

    event.chipInput!.clear();
  } 

  remove(control: FormControl, value: any) {
    const index = (control.value as any[]).indexOf(value);
    if (index >= 0) {
      control.value.splice(index, 1);
      control.updateValueAndValidity();
    }
  }

  public onCategorySelected(title: string) {
    this.spinnerService.show();
    this.categoryApiClient.getCategories(1, 1, false, title)
      .pipe(
        takeUntil(this.unsubscribe),
        tap({
          next: (categoriesList) => {
            if (categoriesList.items) {
              const category = categoriesList.items![0];
              this.gigForm.patchValue({ categoryId: category.id })  
            }
            else {
              throwError('cannot find category with name')
            }
          },
          complete: () => this.spinnerService.hide()
        })
      )
      .subscribe();
  }


  public initPackage() {
    return this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      price: ['', Validators.required],
      packageTier: [0, Validators.required],
      deliveryTime: ['', Validators.required],
      deliveryFrequency: [0, Validators.required],
      revisions: [null],
      revisionType: [0, Validators.required],
    })
  }

  public addPackage() {
    const control = <FormArray>this.gigForm.controls['packages'];
    control.push(this.initPackage());
  }

  public removePackage(i: number) {
    const control = <FormArray>this.gigForm.controls['packages'];
    control.removeAt(i);
  }

}
