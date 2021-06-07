import { ENTER, COMMA } from '@angular/cdk/keycodes';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { MatDialogRef } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, Subject, throwError } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { CategoriesClient, CreateGigCommand, CreatePackageCommand, GigsClient } from 'src/app/clients/gigs-client';
import { FileUpload } from 'src/app/clients/profile-client';

@Component({
  selector: 'app-gig-create',
  templateUrl: './gig-create.component.html',
  styleUrls: ['./gig-create.component.scss']
})
export class GigCreateComponent implements OnInit {
  private unsubscribe = new Subject();
  private gigFormSubmitted = false;
  private gigId: number | undefined;

  isLinear = true;

  gigForm = this.fb.group({
    title: ['', Validators.required],
    description: ['', Validators.required],
    categoryId: ['', Validators.required],
    tags: [[]],
    questions: this.fb.array([ this.initQuestion() ]),
  });

  packagesForm = this.fb.group({
    packages: this.fb.array([])
  })

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
      gigId: [this.gigId, Validators.required],
    })
  }

  public addPackage() {
    const control = <FormArray>this.packagesForm.controls['packages'];
    control.push(this.initPackage());
  }

  public removePackage(i: number) {
    const control = <FormArray>this.packagesForm.controls['packages'];
    control.removeAt(i);
  }

  onPackagesSubmit() {
    debugger;
    const command = CreatePackageCommand.fromJS(this.packagesForm.value)

    this.gigsApiClient.createPackage(this.gigId!, command)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe();
  }

  // ========================

  public upload!: (upload: FileUpload) => Observable<any>;

  constructor(
    public dialogRef: MatDialogRef<GigCreateComponent>,
    private fb: FormBuilder,
    private spinnerService: NgxSpinnerService,
    private gigsApiClient: GigsClient,
    private categoryApiClient: CategoriesClient) { }

  ngOnInit(): void {
  }

  getErrorMessage(key: string, error: string = 'required') {
    const control = this.gigForm.get(key)!;
    if (control.hasError('required')) {
      return 'You must enter a value';
    }

    return control.hasError('email') ? 'Not a valid email' : '';
  }
  
  onNoClick(): void {
    this.dialogRef.close();
  }

  onSubmit() {
    if (!this.gigFormSubmitted) {
      this.gigFormSubmitted = true;
      const command = CreateGigCommand.fromJS(this.gigForm.value);
      this.gigsApiClient.post(command)
        .pipe(
          takeUntil(this.unsubscribe)
        )
        .subscribe(id => {
          this.gigId = id;
          this.addPackage();
          this.upload = (upload: FileUpload) => {
            return this.gigsApiClient.updateImage(id, upload);
          }
        });
    }
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

  // Tags
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

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

  // Event Handlers
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

}
