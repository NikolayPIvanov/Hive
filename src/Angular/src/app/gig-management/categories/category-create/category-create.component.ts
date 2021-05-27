import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CategoriesClient } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-category-create',
  templateUrl: './category-create.component.html',
  styleUrls: ['./category-create.component.scss']
})
export class CategoryCreateComponent implements OnInit {
  private selectedCategoryId: number | undefined;
  form!: FormGroup;

  constructor(
    public formBuilder: FormBuilder,
    public categoriesApiClient: CategoriesClient,
    public dialogRef: MatDialogRef<CategoryCreateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }
  
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      title: ['', Validators.required],
      parentId: [null]
    });
  }

  storeCategoryId($event: any) {
    if ($event.length == 1) {
      this.selectedCategoryId = $event[0].id
      this.form.patchValue({ "parentId": this.selectedCategoryId });
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
