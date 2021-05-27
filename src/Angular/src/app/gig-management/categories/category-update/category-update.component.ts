import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, of, Subscription } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, UpdateCategoryCommand } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-category-update',
  templateUrl: './category-update.component.html',
  styleUrls: ['./category-update.component.scss']
})
export class CategoryUpdateComponent implements OnInit {
  parentCategory$!: Observable<CategoryDto>;

  parentCategory: CategoryDto | undefined = undefined;
  updatedCategory: CategoryDto | undefined;
  form!: FormGroup;

  constructor(
    public formBuilder: FormBuilder,
    public categoriesApiClient: CategoriesClient,
    public dialogRef: MatDialogRef<CategoryUpdateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CategoryDto) { }
  
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      id: [this.data.id, Validators.required],
      title: [this.data.title, Validators.required],
      parentId: [this.data.parentId]
    });

    if (this.data.parentId) {
      this.parentCategory$ = this.categoriesApiClient.getCategoryById(+this.data.parentId!)
      .pipe(
        map((category: CategoryDto) => {
          if (category && category.parentId == null) {
            this.parentCategory == category;
          }

          return category;
        }));
    }
  }

  onSubmit() {
    let data: any = {};
    for (let key in this.form.controls) {
      data[key] = this.form.controls[key].value
    }
    const command = UpdateCategoryCommand.fromJS(data)
    
    this.categoriesApiClient.updateCategory(command.id!, command)
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
