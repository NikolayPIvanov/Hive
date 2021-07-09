import { ENTER, COMMA } from '@angular/cdk/keycodes';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { MatDialogRef } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, of, Subject, throwError } from 'rxjs';
import { map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, CreateGigCommand, DeliveryFrequency, FileUpload, GigsClient, PackageTier, RevisionType } from 'src/app/clients/gigs-client';
import { PlanDto } from 'src/app/clients/investing-client';

@Component({
  selector: 'app-gig-create',
  templateUrl: './gig-create.component.html',
  styleUrls: ['./gig-create.component.scss']
})
export class GigCreateComponent implements OnInit {
  public packageTiers = [PackageTier.Basic, PackageTier.Standard, PackageTier.Premium]
  displayTier(tier: PackageTier) {
    return PackageTier[tier];
  }

  public deliveryFrequencies = [DeliveryFrequency.Hours, DeliveryFrequency.Days, DeliveryFrequency.Weeks]
  displayFrequency(f: DeliveryFrequency) {
    return DeliveryFrequency[f];
  }

  public revisionsTypes = [RevisionType.Unlimited, RevisionType.Numeric]
  displayRevision(f: RevisionType) {
    return RevisionType[f];
  }

  // Shared
  private unsubscribe = new Subject();
  isLinear = true;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  constructor(
    public dialogRef: MatDialogRef<GigCreateComponent>,
    private fb: FormBuilder,
    private gigsApiClient: GigsClient) { }

  ngOnInit(): void { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  setImage(base64: string) {
    this.gigForm.patchValue({ image: base64 });
  }

  // Gig Main Information
  gigForm = this.fb.group({
    title: ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(100)])],
    description: ['', Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(2500)])],
    categoryId: ['', Validators.required],
    planId: [null],
    tags: [[]],
    questions: this.fb.array([]),
    image: [null],
    packages: this.fb.array([ this.initPackage() ]),
  });

  public get packagesForm() {
    return (this.gigForm.get('packages') as FormArray)?.controls;
  }

  private gid: number | undefined;

  onSubmit() {
    const command = CreateGigCommand.fromJS(this.gigForm.value);
      this.gigsApiClient.createGig(command)
        .pipe(
          takeUntil(this.unsubscribe),
          switchMap((id) => {
            const imageData = this.gigForm.get('image');
            this.gid = id;
            if (imageData?.value) {
              this.gid = id;
              return this.gigsApiClient.updateImage(
                id, FileUpload.fromJS({ fileData: imageData.value }))
            }

            return of(id);
          }),
          tap({ complete: () => this.dialogRef.close(this.gid) }))
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

  public onCategorySelected(category: CategoryDto) {
    if (category) {
      this.gigForm.patchValue({ categoryId: category.id });
    }
  }

  public setPlan(plan: PlanDto | undefined) {
    if (plan) {
      this.gigForm.patchValue({ planId: plan.id!})
    }
  }

  public initPackage() {
    return this.fb.group({
      title: ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(50)])],
      description: ['', Validators.compose([Validators.required, Validators.minLength(10), Validators.maxLength(200)])],
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
