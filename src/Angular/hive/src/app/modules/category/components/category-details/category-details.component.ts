import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { CategoriesClient, CategoryDto, UpdateCategoryCommand } from 'src/app/clients/gigs-client';

export interface Section {
  name: string;
  updated: Date;
}

@Component({
  selector: 'app-category-details',
  templateUrl: './category-details.component.html',
  styleUrls: ['./category-details.component.scss']
})
export class CategoryDetailsComponent implements OnInit {
  public editMode: boolean = false;

  categoryForm = this.fb.group({
    id: ['', Validators.required],
    title: ['', Validators.required],
    description: ['', Validators.required],
    parentId: [null]
  });

  constructor(
    private fb: FormBuilder,
    private categoryApiClient: CategoriesClient,
    private spinner: NgxSpinnerService,
    public dialogRef: MatDialogRef<CategoryDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public category: CategoryDto) { }

  ngOnInit(): void {
    this.categoryForm.patchValue(this.category);
    this.categoryForm.patchValue({ parentId: this.category.parentOverview?.id})
  }

  onSelected(category: CategoryDto | undefined) {
    const item = category ? category.id! : undefined;
    this.categoryForm.patchValue({ parentId: item})
  }

  onSubmit(): void {
    this.spinner.show()
    const command = UpdateCategoryCommand.fromJS(this.categoryForm.value)
    this.categoryApiClient.updateCategory(this.category.id!, command)
      .pipe(
        switchMap(id => this.categoryApiClient.getCategoryById(command.id!) ),
        tap({
          next: (category) => this.dialogRef.close(category),
          complete: () => {
            this.spinner.hide();
          }
        })
      )
      .subscribe();
  }

}
