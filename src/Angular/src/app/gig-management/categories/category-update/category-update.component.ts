import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { map, switchMap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, UpdateCategoryCommand } from 'src/app/clients/gigs-client';
import { CategoryCreateComponent } from '../category-create/category-create.component';

@Component({
  selector: 'app-category-update',
  templateUrl: './category-update.component.html',
  styleUrls: ['./category-update.component.scss']
})
export class CategoryUpdateComponent implements OnInit {
  parentDto: CategoryDto | undefined;
  updatedCategory: CategoryDto | undefined;
  form!: FormGroup;

  constructor(
    public formBuilder: FormBuilder,
    public categoriesApiClient: CategoriesClient,
    public dialogRef: MatDialogRef<CategoryCreateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }
  
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      id: [0, Validators.required],
      title: ['', Validators.required],
      parentId: [null]
    });

    this.categoriesApiClient.getCategoryById(+this.data.id)
      .pipe(switchMap((dto: CategoryDto) => {
        this.form.patchValue(dto);
        if (dto.parentId) {
          return this.categoriesApiClient.getCategoryById(dto.parentId);
        }
      }))
      .subscribe((dto: CategoryDto) => );
    
    this.categoriesApiClient.getCategoryById()
  }

  onSubmit() {
    debugger;
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
    debugger;
    this.parentDto = undefined;
    if ($event.length == 1) {
      this.parentDto = $event[0]
      this.form.controls['parentId'].setValue(this.parentDto?.id);
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
