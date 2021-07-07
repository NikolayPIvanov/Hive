import { ENTER, COMMA } from '@angular/cdk/keycodes';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, of, Subject } from 'rxjs';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, DeliveryFrequency, FileUpload, GigDto, GigsClient, PackageTier, RevisionType, UpdateGigCommand } from 'src/app/clients/gigs-client';
import { FileResponse, ProfileClient } from 'src/app/clients/profile-client';

@Component({
  selector: 'app-gig-edit',
  templateUrl: './gig-edit.component.html',
  styleUrls: ['./gig-edit.component.scss']
})
export class GigEditComponent implements OnInit {
  gigForm = this.fb.group({
    id: [this.data.id!, Validators.required],
    title: [this.data.title!, Validators.required],
    description: [this.data.description!, Validators.required],
    categoryId: [this.data.categoryId!, Validators.required],
    planId: [this.data.planId!],
    tags: [this.data.tags!],
    image: [this.data.imagePath?.path!],
    questions: this.fb.array([]),
    packages: this.createPackages(),
  });

  compareFunction(o1: any, o2: any) {
    return (o1 == o2);
  }

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

  private unsubscribe = new Subject();
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  constructor(
    public dialogRef: MatDialogRef<GigEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: GigDto,
    private fb: FormBuilder,
    private gigsApiClient: GigsClient,
    private profileClient: ProfileClient,
    private spinnerService: NgxSpinnerService) { }
  
  private createPackages() {
    const controls = this.data.packages?.map(p => {
      let packageForm = this.initPackage();
      packageForm.patchValue(p);
      return packageForm;
    })


    return this.fb.array(controls!)
  }

  public get packagesForm() {
    return (this.gigForm.get('packages') as FormArray)?.controls;
  }

  public download!: Observable<FileResponse>;
  setImage(base64: string) {
    this.gigForm.patchValue({ image: base64 });
  }

  ngOnInit(): void {
    this.download = this.gigsApiClient.getAvatar(this.data.id!)
  }

  onCategorySelected(category: CategoryDto) {
    if (category) {
      this.gigForm.patchValue({ categoryId: category.id });
    }
  }

  onSubmit() {
    this.spinnerService.show();
    const command = UpdateGigCommand.fromJS(this.gigForm.value)
    this.gigsApiClient.update(this.data.id!, command)
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(x => {
          const imageData = this.gigForm.get('image');
          if (imageData?.value && imageData?.value != this.data.imagePath?.path) {
            return this.gigsApiClient.updateImage(this.data.id!, FileUpload.fromJS({ fileData: imageData.value }))
          }
          return of(x);
        }),
        tap({
          complete: () => {
            this.spinnerService.hide();
            this.dialogRef.close();
          }
        }))
      .subscribe();
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

  addQuestion() {
    const control = <FormArray>this.gigForm.controls['questions'];
    control.push(this.initQuestion());
  }

  removeQuestion(i: number) {
    const control = <FormArray>this.gigForm.controls['questions'];
    control.removeAt(i);
  }

  public addPackage() {
    const control = <FormArray>this.gigForm.controls['packages'];
    control.push(this.initPackage());
  }

  public removePackage(i: number) {
    const control = <FormArray>this.gigForm.controls['packages'];
    control.removeAt(i);
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


  private initQuestion() {
    return this.fb.group({
        title: ['', Validators.required],
        answer: ['']
    });
  }

}
