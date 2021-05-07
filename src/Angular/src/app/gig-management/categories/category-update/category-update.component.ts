import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, of, Subscription } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, UpdateCategoryCommand } from 'src/app/clients/gigs-client';
import { CategoryCreateComponent } from '../category-create/category-create.component';

@Component({
  selector: 'app-category-update',
  templateUrl: './category-update.component.html',
  styleUrls: ['./category-update.component.scss']
})
export class CategoryUpdateComponent implements OnInit {
  
  private updateSubscription!: Subscription;
  parentCategory$!: Observable<CategoryDto>;

  parentCategory: CategoryDto | undefined;
  updatedCategory: CategoryDto | undefined;
  form!: FormGroup;


  constructor(
    public formBuilder: FormBuilder,
    public categoriesApiClient: CategoriesClient,
    public dialogRef: MatDialogRef<CategoryUpdateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }
  
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      id: [0, Validators.required],
      title: ['', Validators.required],
      parentId: [null]
    });

    this.form.patchValue(this.data.entity);

    this.parentCategory$ = this.categoriesApiClient.getCategoryById(+this.data.entity.parentId)
      .pipe(
        map((category: CategoryDto) => {
          if (category && category.parentId == null) {
            this.parentCategory == category;
          }

          return category;
        }));
  }

  onSubmit() {
    let data: any = {};
    for (let key in this.form.controls) {
      data[key] = this.form.controls[key].value
    }
    const command = UpdateCategoryCommand.fromJS(data)
    
    this.updateSubscription = this.categoriesApiClient.updateCategory(command.id!, command)
      .pipe(switchMap(_ => this.categoriesApiClient.getCategoryById(command.id!)))
      .subscribe((category: CategoryDto) => this.updatedCategory = category)
  }

  storeCategoryId($event: any) {
    if ($event.length == 1) {
      this.parentCategory = $event[0]
      this.form.controls['parentId'].setValue(this.parentCategory?.id);
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
